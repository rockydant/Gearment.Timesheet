using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Gearment.Timesheet.Models;
using Gearment.Timesheet.Repository;
using Oqtane.Repository;
using Oqtane.Security;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Gearment.Employee.Repository;
using Gearment.Department.Repository;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using RestWrapper;
using System.Threading.Tasks;
using RestSharp;
using Gearment.GearmentSetting.Repository;
using System.Globalization;

namespace Gearment.Timesheet.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _TimesheetRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;
        private readonly IFileRepository _files;
        private readonly IFolderRepository _folders;
        private readonly IUserPermissions _userPermissions;
        private readonly IWebHostEnvironment _environment;
        private readonly ITenantResolver _tenants;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IGearmentSettingRepository _settingRepository;

        public TimesheetController(ITimesheetRepository TimesheetRepository, IEmployeeRepository EmployeeRepository, IFolderRepository Folders, IDepartmentRepository DepartmentRepository, IWebHostEnvironment environment, ITenantResolver tenants, IUserPermissions userPermissions, IGearmentSettingRepository settingRepository, IFileRepository files, ILogManager logger, IHttpContextAccessor accessor)
        {
            _TimesheetRepository = TimesheetRepository;
            _logger = logger;
            _files = files;
            _userPermissions = userPermissions;
            _environment = environment;
            _tenants = tenants;
            _employeeRepository = EmployeeRepository;
            _departmentRepository = DepartmentRepository;
            _settingRepository = settingRepository;
            _folders = Folders;

            if (accessor.HttpContext.Request.Query.ContainsKey("entityid"))
            {
                _entityId = int.Parse(accessor.HttpContext.Request.Query["entityid"]);
            }
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.Timesheet> Get(string moduleid)
        {
            return _TimesheetRepository.GetTimesheets(int.Parse(moduleid));
        }

        // GET api/<controller>/5
        [HttpGet("process/{moduleId}/{id}")]
        public Models.TimesheetViewModel Process(int moduleId, int id)
        {
            Oqtane.Models.File file = _files.GetFile(id);
            List<Models.Timesheet> timesheetList = new List<Models.Timesheet>();
            Models.TimesheetViewModel timesheetViewModel = new Models.TimesheetViewModel();
            List<Models.Timesheet> result = new List<Models.Timesheet>();
            List<Employee.Models.Employee> missingEmployeeList = new List<Employee.Models.Employee>();

            if (file != null)
            {
                if (_userPermissions.IsAuthorized(User, PermissionNames.View, file.Folder.Permissions))
                {
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IApplication application = excelEngine.Excel;
                        application.DefaultVersion = ExcelVersion.Excel2016;
                        FileStream inputStream = new FileStream(ResolveApplicationPath(file), FileMode.Open, FileAccess.Read);
                        IWorkbook workbook = application.Workbooks.Open(inputStream, ExcelParseOptions.Default);
                        IWorksheet worksheet = workbook.Worksheets[0];
                        //Read data from spreadsheet.
                        DataTable dataTable = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                        timesheetList = ExportDataFromExcelSheet(worksheet, 2, 1, dataTable.Rows.Count + 1, moduleId);
                    }

                    // Add Raw Data from Devices
                    foreach (var item in timesheetList)
                    {
                        var row = _TimesheetRepository.AddTimesheet(item);
                        if (row != null)
                        {
                            result.Add(row);
                        }
                    }

                    // Process Data: Manipulate Timesheet Records, Add New Employees
                    if (result.Any())
                    {
                        // Manipulate Timesheet records
                        List<TimesheetDataViewModel> dataViewModel = new List<TimesheetDataViewModel>();

                        foreach (var item in result)
                        {
                            var foundRecord = dataViewModel.FirstOrDefault(x => x.FirstName == item.FirstName && x.LastName == item.LastName && x.Date == item.Date);
                            if (foundRecord != null)
                            {
                                if (!string.IsNullOrEmpty(item.In))
                                {
                                    foundRecord.InRecords.Add(DateTime.Parse(item.In));
                                }

                                if (!string.IsNullOrEmpty(item.Out))
                                {
                                    foundRecord.OutRecords.Add(DateTime.Parse(item.Out));
                                }

                                if (item.Hours != null)
                                {
                                    foundRecord.Hours.Add(decimal.Parse(item.Hours.Split(':').First()) + (decimal.Parse(item.Hours.Split(':').Last()) / 60));
                                }
                            }
                            else
                            {
                                foundRecord = new TimesheetDataViewModel();
                                foundRecord.InRecords = new List<DateTime>();
                                foundRecord.OutRecords = new List<DateTime>();

                                foundRecord.FirstName = item.FirstName;
                                foundRecord.LastName = item.LastName;
                                foundRecord.PayrollID = item.PayRollID;
                                foundRecord.Date = item.Date;
                                foundRecord.DayOfWeek = item.DayOfWeek;

                                if (!string.IsNullOrEmpty(item.In))
                                {
                                    foundRecord.InRecords.Add(DateTime.Parse(item.In));
                                }

                                if (!string.IsNullOrEmpty(item.Out))
                                {
                                    foundRecord.OutRecords.Add(DateTime.Parse(item.Out));
                                }


                                if (item.Hours != null)
                                {
                                    foundRecord.Hours = new List<decimal>();
                                    foundRecord.Hours.Add(decimal.Parse(item.Hours.Split(':').First()) + (decimal.Parse(item.Hours.Split(':').Last()) / 60));
                                }

                                dataViewModel.Add(foundRecord);
                            }
                        }

                        // Add new Employee
                        foreach (var item in dataViewModel)
                        {
                            TimesheetData timesheetData = new TimesheetData();

                            Employee.Models.Employee employee = new Employee.Models.Employee();
                            employee.Name = item.FirstName + " " + item.LastName;

                            if (!string.IsNullOrEmpty(item.PayrollID))
                            {
                                employee.PayrollID = int.Parse(item.PayrollID);
                            }

                            // Check Existed Employee
                            employee = _employeeRepository.GetEmployeeByName(employee.Name);

                            if (employee == null)
                            {
                                Employee.Models.Employee missing = new Employee.Models.Employee();
                                missing.Name = item.FirstName.Trim() + " " + item.LastName.Trim();
                                missing.PayrollID = string.IsNullOrEmpty(item.PayrollID) ? 0 : int.Parse(item.PayrollID);
                                missing.Rate = -1;
                                missing.Department = string.Empty;
                                missing.StartDate = DateTime.UtcNow;
                                missing.Status = "Active";
                                missing.Note = string.Empty;
                                missing.ModuleId = moduleId;

                                employee = _employeeRepository.AddEmployee(missing);
                                missingEmployeeList.Add(employee);
                            }
                            else
                            {
                                employee.PayrollID = string.IsNullOrEmpty(item.PayrollID) ? 0 : int.Parse(item.PayrollID);
                                employee = _employeeRepository.UpdateEmployee(employee);
                            }

                            timesheetData.FirstName = item.FirstName.Trim();
                            timesheetData.LastName = item.LastName.Trim();

                            timesheetData.PayrollID = item.PayrollID;
                            timesheetData.Rate = employee.Rate;
                            timesheetData.Date = item.Date;
                            timesheetData.DayOfWeek = item.DayOfWeek;
                            timesheetData.Department = employee.Department;

                            DateTime limitStartTime = DateTime.Parse("2012/12/12 10:00:00.000");
                            DateTime nightShiftStartTime = DateTime.Parse("2012/12/12 13:30:00.000");
                            DateTime limitEndTime = DateTime.Parse("2012/12/12 19:00:00.000");

                            if (item.InRecords.Any() && item.OutRecords.Any())
                            {
                                if ((item.InRecords.Min().TimeOfDay < limitStartTime.TimeOfDay && item.InRecords.Max().TimeOfDay > limitEndTime.TimeOfDay) || (item.InRecords.Min().TimeOfDay > nightShiftStartTime.TimeOfDay && item.InRecords.Max().TimeOfDay > limitEndTime.TimeOfDay))
                                {
                                    timesheetData.DailyStartTime = item.InRecords.Min();
                                    timesheetData.DailyEndTime = item.OutRecords.Last();

                                    timesheetData.TotalWorkingHour = (decimal)Math.Round((item.OutRecords.Last() - item.InRecords.Min()).TotalMinutes / 60, 1);

                                    timesheetData.TotalRestHour = (decimal)Math.Round((item.OutRecords.Last() - item.InRecords.Min()).TotalMinutes / 60, 1) - item.Hours.Sum();
                                }
                                else
                                {
                                    if (item.InRecords.Any())
                                    {
                                        timesheetData.DailyStartTime = item.InRecords.Min();
                                        if (item.InRecords.Count > 1)
                                        {
                                            timesheetData.BreakEndTime = item.InRecords.Max();
                                        }
                                    }

                                    if (item.OutRecords.Any())
                                    {
                                        timesheetData.DailyEndTime = item.OutRecords.Max();
                                        if (item.OutRecords.Count > 1)
                                        {
                                            timesheetData.BreakStartTime = item.OutRecords.Min();
                                        }

                                    }

                                    if (timesheetData.BreakEndTime != null || timesheetData.BreakStartTime != null)
                                    {
                                        //timesheetData.TotalRestHour = timesheetData.BreakEndTime.Hour - timesheetData.BreakStartTime.Hour;
                                        timesheetData.TotalRestHour = (decimal)Math.Round((timesheetData.BreakEndTime - timesheetData.BreakStartTime).TotalMinutes / 60, 1);

                                    }
                                    else
                                    {
                                        timesheetData.TotalRestHour = 0;
                                    }

                                    if (timesheetData.DailyEndTime != null || timesheetData.DailyStartTime != null)
                                    {
                                        //timesheetData.TotalRestHour = timesheetData.BreakEndTime.Hour - timesheetData.BreakStartTime.Hour;                                
                                        double startTime = timesheetData.DailyStartTime.TimeOfDay.Hours;

                                        if (timesheetData.DailyStartTime.TimeOfDay.Minutes >= 45)
                                        {
                                            startTime += 1;
                                        }
                                        else if (timesheetData.DailyStartTime.TimeOfDay.Minutes > 15 && timesheetData.DailyStartTime.TimeOfDay.Minutes < 45)
                                        {
                                            startTime += 0.5;
                                        }

                                        double endTime = timesheetData.DailyEndTime.TimeOfDay.Hours;

                                        if (timesheetData.DailyEndTime.TimeOfDay.Minutes >= 45)
                                        {
                                            endTime += 1;
                                        }
                                        else if (timesheetData.DailyEndTime.TimeOfDay.Minutes > 15 && timesheetData.DailyEndTime.TimeOfDay.Minutes < 45)
                                        {
                                            endTime += 0.5;
                                        }

                                        timesheetData.TotalWorkingHour = (decimal)(endTime - startTime);
                                    }
                                    else
                                    {
                                        timesheetData.TotalWorkingHour = 0;
                                    }
                                }
                            }



                            timesheetData.Status = employee.Status;

                            timesheetData.CreatedBy = User.Identity.Name;
                            timesheetData.CreatedOn = DateTime.Now;
                            timesheetData.ModifiedBy = User.Identity.Name;
                            timesheetData.ModifiedOn = DateTime.Now;
                            timesheetData.Notes = string.Empty;
                            timesheetData.TotalBreakHour = 0;
                            _TimesheetRepository.AddTimesheetData(timesheetData);

                            //// check if exist data of that day (login but not logout)
                            //var foundRecord = _TimesheetRepository.GetTimesheetData(timesheetData);
                            //if (foundRecord != null)
                            //{
                            //    _TimesheetRepository.UpdateTimesheetData(timesheetData);
                            //}
                            //else
                            //{
                            //    _TimesheetRepository.AddTimesheetData(timesheetData);
                            //}
                        }
                    }

                    timesheetViewModel.TimesheetList = result;
                    timesheetViewModel.MissingEmployee = missingEmployeeList;

                    return timesheetViewModel;
                }
                else
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Read, "User Not Authorized To Access File {File}", file);
                    HttpContext.Response.StatusCode = 401;
                    return null;
                }
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "File Not Found {FileId}", id);
                HttpContext.Response.StatusCode = 404;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.TimesheetData Get(int id)
        {
            return _TimesheetRepository.GetTimesheet(id);
        }

        [HttpGet("data")]
        public List<Models.TimesheetData> GetTimesheetData()
        {
            return _TimesheetRepository.GetAllTimesheetData();
        }

        [HttpPost("data")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public List<Models.TimesheetData> GetTimesheetDataByDate([FromBody] TimesheetDailyQuery Query)
        {
            if (Query.Department == "All")
            {
                return _TimesheetRepository.GetAllTimesheetData().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= Query.ToDate).ToList();
            }
            else
            {
                return _TimesheetRepository.GetAllTimesheetData().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= Query.ToDate && x.Department == Query.Department).ToList();
            }
        }

        [HttpGet("attendance/event/{eventId}")]
        public async Task<List<Models.Employee_FaceRegEventDetail>> GetEmployeeByEvenIdAsync(int eventId)
        {
            List<Models.Employee_FaceRegEventDetail> result = new List<Employee_FaceRegEventDetail>();

            RestWrapper.RestRequest apiConsume = new RestWrapper.RestRequest("https://attendance.geatech.net/api/face/event/" + eventId, RestWrapper.HttpMethod.GET);
            RestWrapper.RestResponse apiResponse = await apiConsume.SendAsync();
            if (apiResponse.StatusCode == 200)
            {
                List<API_FaceMatch> foundDataList = apiResponse.DataFromJson<List<API_FaceMatch>>();
                if (foundDataList.Any())
                {
                    foundDataList = foundDataList.GroupBy(x => x.EmployeeId).Select(y => y.FirstOrDefault()).ToList();
                    foreach (var item in foundDataList)
                    {
                        Employee_FaceRegEventDetail employeeDetail = new Employee_FaceRegEventDetail();

                        var employeFaceReg = _TimesheetRepository.GetEmployee_FaceReg(int.Parse(item.EmployeeId));
                        var info = _employeeRepository.GetEmployee(int.Parse(item.EmployeeId));
                        employeeDetail.EmployeeId = employeFaceReg.EmployeeId;
                        employeeDetail.Department = info.Department;
                        employeeDetail.PhotoJpg = employeFaceReg.FacePhoto1;
                        employeeDetail.Name = info.Name;

                        result.Add(employeeDetail);
                    }
                }
            }

            return result;
        }

        [HttpGet("attendance/employee/{empId}")]
        public Models.Employee_FaceReg GetEmployeeFaceById(string empId)
        {
            return _TimesheetRepository.GetFaces(empId);

        }

        [HttpPost("attendance")]
        public List<Models.TimesheetDataExcelExport> GetAttendanceData([FromBody] TimesheetDailyQuery Query)
        {
            return QueryPayrollData(Query);
        }

        [HttpPost("payroll")]
        public List<Models.PayrollViewModel> GetPayrollData([FromBody] TimesheetDailyQuery Query)
        {
            List<PayrollViewModel> result = new List<PayrollViewModel>();
            List<TimesheetDataExcelExport> summary = new List<TimesheetDataExcelExport>();
            summary = QueryPayrollData(Query);

            if (summary.Any())
            {
                foreach (var item in summary)
                {
                    var IsExisted = result.FirstOrDefault(x => x.Name == item.Name && x.Department == item.Department);

                    PayrollDetailViewModel newRecord = new PayrollDetailViewModel();
                    newRecord.DayOfWeek = item.DayOfWeek;
                    newRecord.Date = DateTime.Parse(item.Date);
                    newRecord.DailyStartTime = item.DailyStartTime;
                    newRecord.DailyEndTime = item.DailyEndTime;
                    newRecord.BreakStartTime = item.BreakStartTime;
                    newRecord.BreakEndTime = item.BreakEndTime;
                    newRecord.TotalBreakHourCurrentDay = item.TotalRestHour;
                    newRecord.TotalWorkingHourCurrentDay = item.TotalWorkingHour;
                    newRecord.TotalOvertimeHourCurrentDay = 0;

                    //if (Query.Holidays.Any(x => x.Date == DateTime.Parse(newRecord.Date)))
                    //{
                    //    newRecord.TotalBonusHourCurrentDay = (decimal)Query.Holidays.FirstOrDefault(x => x.Date == DateTime.Parse(newRecord.Date)).BonusHour;
                    //}

                    if (IsBusinessDay(newRecord.Date))
                    {
                        if (newRecord.TotalWorkingHourCurrentDay >= 8)
                        {
                            newRecord.TotalOvertimeHourCurrentDay = newRecord.TotalWorkingHourCurrentDay - 8;
                            newRecord.TotalWorkingHourCurrentDay = 8;
                        }
                    }
                    else
                    {
                        newRecord.TotalOvertimeHourCurrentDay = newRecord.TotalWorkingHourCurrentDay;
                        newRecord.TotalWorkingHourCurrentDay = 0;
                    }

                    if (Query.SickLeaves.Any(x => x.Name == item.Name && x.Department == item.Department && x.Date == newRecord.Date))
                    {
                        newRecord.TotalWorkingHourCurrentDay = 0;
                    }

                    newRecord.TotalOvertimePayCurrentDay = item.Rate * (decimal)1.5 * newRecord.TotalOvertimeHourCurrentDay;
                    newRecord.TotalPayCurrentDay = item.Rate * newRecord.TotalWorkingHourCurrentDay;

                    if (IsExisted != null)
                    {
                        newRecord.TotalSickPayCurrentDay = 0;
                        IsExisted.PayrollDetailList.Add(newRecord);
                        IsExisted.TotalPay += newRecord.TotalPayCurrentDay;
                        IsExisted.TotalOvertimePay += newRecord.TotalOvertimePayCurrentDay;
                        IsExisted.TotalSickPay += newRecord.TotalSickPayCurrentDay;

                        IsExisted.TotalWorkingHours += newRecord.TotalWorkingHourCurrentDay;
                        IsExisted.TotalBreakHours += newRecord.TotalBreakHourCurrentDay;
                        IsExisted.TotalOvertimeHours += newRecord.TotalOvertimeHourCurrentDay;

                        IsExisted.PayrollDetailList.OrderBy(x => x.Date);
                    }
                    else
                    {
                        PayrollViewModel newItem = new PayrollViewModel();
                        newItem.Id = item.EmployeeId;
                        newItem.Name = item.Name;
                        newItem.Rate = item.Rate;
                        newItem.Department = item.Department;
                        newItem.BonusRate = item.Rate;
                        newItem.TotalWorkingHours = 0;
                        newItem.TotalBreakHours = 0;
                        newItem.TotalOvertimeHours = 0;
                        newItem.TotalPay = 0;
                        newItem.TotalBonusPay = 0;
                        newItem.TotalOvertimePay = 0;
                        newItem.TotalSickPay = 0;
                        newItem.PayrollDetailList = new List<PayrollDetailViewModel>();

                        newRecord.TotalSickPayCurrentDay = 0;

                        newItem.TotalPay += newRecord.TotalPayCurrentDay;
                        newItem.TotalOvertimePay += newRecord.TotalOvertimePayCurrentDay;
                        newItem.TotalSickPay += newRecord.TotalSickPayCurrentDay;

                        newItem.TotalWorkingHours += newRecord.TotalWorkingHourCurrentDay;
                        newItem.TotalBreakHours += newRecord.TotalBreakHourCurrentDay;
                        newItem.TotalOvertimeHours += newRecord.TotalOvertimeHourCurrentDay;

                        newItem.PayrollDetailList.Add(newRecord);

                        result.Add(newItem);
                    }
                }               

                foreach (var item in Query.SickLeaves)
                {
                    var employee = result.FirstOrDefault(x => x.Name == item.Name && x.Department == item.Department);
                    if (employee != null)
                    {
                        PayrollDetailViewModel sickLeaveRecord = new PayrollDetailViewModel();
                        sickLeaveRecord.TotalWorkingHourCurrentDay = 0;
                        sickLeaveRecord.TotalBreakHourCurrentDay = 0;
                        sickLeaveRecord.TotalOvertimeHourCurrentDay = 0;
                        sickLeaveRecord.TotalPayCurrentDay = 0;
                        sickLeaveRecord.TotalOvertimePayCurrentDay = 0;
                        //sickLeaveRecord.TotalBonusPayCurrentDay = 0;
                        sickLeaveRecord.TotalSickHourCurrentDay = (decimal)item.Hours;
                        sickLeaveRecord.TotalSickPayCurrentDay = (decimal)item.Hours * employee.Rate;
                        sickLeaveRecord.Date = item.Date;
                        sickLeaveRecord.DayOfWeek = item.Date.DayOfWeek.ToString();

                        employee.PayrollDetailList.Add(sickLeaveRecord);
                        employee.PayrollDetailList = employee.PayrollDetailList.OrderBy(x => x.Date).ToList();
                        employee.TotalSickHours += sickLeaveRecord.TotalSickHourCurrentDay;
                        employee.TotalSickPay += sickLeaveRecord.TotalSickPayCurrentDay;
                    }
                }

                foreach (var item in result)
                {
                    foreach (var holiday in Query.Holidays)
                    {
                        if (item.PayrollDetailList.First().Date <= holiday.Date && item.PayrollDetailList.Last().Date >= holiday.Date)
                        {
                            var existed = item.PayrollDetailList.FirstOrDefault(x => x.Date == holiday.Date);
                            if (existed != null)
                            {
                                existed.TotalBonusHourCurrentDay = (decimal)holiday.BonusHour;
                                existed.TotalBonusPayCurrentDay = (decimal)(item.Rate * holiday.BonusHour);
                            }
                            else
                            {
                                PayrollDetailViewModel holidayRecord = new PayrollDetailViewModel();
                                holidayRecord.TotalWorkingHourCurrentDay = 0;
                                holidayRecord.TotalBreakHourCurrentDay = 0;
                                holidayRecord.TotalOvertimeHourCurrentDay = 0;
                                holidayRecord.TotalPayCurrentDay = 0;
                                holidayRecord.TotalOvertimePayCurrentDay = 0;
                                holidayRecord.TotalSickHourCurrentDay = 0;
                                holidayRecord.TotalSickPayCurrentDay = 0;
                                holidayRecord.Date = holiday.Date;
                                holidayRecord.DayOfWeek = holiday.Date.DayOfWeek.ToString();

                                holidayRecord.TotalBonusPayCurrentDay = (decimal)(item.Rate * holiday.BonusHour);
                                holidayRecord.TotalBonusHourCurrentDay = (decimal)holiday.BonusHour;


                                item.PayrollDetailList.Add(holidayRecord);
                                item.PayrollDetailList = item.PayrollDetailList.OrderBy(x => x.Date).ToList();
                            }

                            item.TotalBonusPay += (decimal)(item.Rate * holiday.BonusHour);
                            item.TotalBonusHours += (decimal)holiday.BonusHour;
                        }
                    }
                }
            }

            return result;
        }

        private bool IsBusinessDay(DateTime date)
        {
            return
                date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday;
        }

        private List<Models.TimesheetDataExcelExport> QueryPayrollData(TimesheetDailyQuery Query)
        {
            var data = _TimesheetRepository.GetAllEmployee_FaceRegEvent(Query);

            if (Query.Department != "All")
            {
                data = data.Where(x => x.Department == Query.Department).ToList();
            }

            List<TimesheetDataExcelExport> summary = new List<TimesheetDataExcelExport>();

            foreach (var item in data)
            {
                var foundItem = summary.FirstOrDefault(x => x.EmployeeId == item.EmployeeId && DateTime.Parse(x.Date) == item.EventTime.Date);
                if (foundItem != null)
                {
                    foundItem.EventTimeLine.Add(item);
                }
                else
                {
                    TimesheetDataExcelExport newItem = new TimesheetDataExcelExport();
                    newItem.Name = item.Name;
                    newItem.EmployeeId = item.EmployeeId;
                    newItem.Department = item.Department;
                    newItem.Date = item.EventTime.Date.ToString("MM/dd/yyyy");
                    newItem.EventTimeLine = new List<Employee_FaceRegEventDetail>();
                    newItem.EventTimeLine.Add(item);
                    newItem.DayOfWeek = item.EventTime.DayOfWeek.ToString();
                    newItem.Rate = item.Rate;
                    newItem.PayrollID = item.PayrollID.ToString();

                    summary.Add(newItem);
                }
            }

            if (Query.IsMultiCheckin)
            {
                summary = summary.Where(x => x.EventTimeLine.Count(y => y.EventType == "In") > 1 && x.EventTimeLine.Count(y => y.EventType == "Out") > 1).ToList();
            }

            foreach (var item in summary)
            {
                if (item.EventTimeLine != null)
                {
                    item.EventTimeLine = item.EventTimeLine.OrderBy(x => x.EventTime).ToList();

                    if (item.EventTimeLine.Count() > 1)
                    {
                        var checkpointList = new List<Employee_FaceRegEventDetail>();

                        if (item.EventTimeLine.Any())
                        {
                            if (item.EventTimeLine.FirstOrDefault().EventType != "In")
                            {
                                checkpointList.Add(item.EventTimeLine.FirstOrDefault());
                                item.EventTimeLine.Remove(item.EventTimeLine.FirstOrDefault());
                            }
                        }

                        if (checkpointList.Any())
                        {
                            var foundRecord = summary.Where(x => x.EmployeeId == item.EmployeeId && DateTime.Parse(x.Date) == checkpointList.First().EventTime.Date.AddDays(-1)).OrderBy(x => DateTime.Parse(x.Date)).FirstOrDefault();
                            if (foundRecord != null)
                            {
                                foundRecord.EventTimeLine.AddRange(checkpointList);
                            }
                        }
                    }
                }
            }

            foreach (var item in summary)
            {
                if (item.EventTimeLine != null)
                {
                    var inList = item.EventTimeLine.Where(x => x.EventType == "In").OrderBy(x => x.EventTime).ToList();
                    var outList = item.EventTimeLine.Where(x => x.EventType == "Out").OrderBy(x => x.EventTime).ToList();
                    var breakList = item.EventTimeLine.Where(x => x.EventType == "Break").OrderBy(x => x.EventTime).ToList();
                    var endBreakList = item.EventTimeLine.Where(x => x.EventType == "End-Break").OrderBy(x => x.EventTime).ToList();

                    if (inList.Any())
                    {
                        item.DailyStartTime = inList.First().EventTime.ToString("hh:mm tt");
                    }
                    else
                    {
                        item.DailyStartTime = "N/A";
                    }

                    if (outList.Any())
                    {
                        item.DailyEndTime = outList.Last().EventTime.ToString("hh:mm tt");
                    }
                    else
                    {
                        item.DailyEndTime = "N/A";
                    }

                    if (breakList.Any())
                    {
                        item.BreakStartTime = breakList.First().EventTime.ToString("hh:mm tt");
                    }
                    else
                    {
                        item.BreakStartTime = "N/A";
                    }

                    if (endBreakList.Any())
                    {
                        item.BreakEndTime = endBreakList.Last().EventTime.ToString("hh:mm tt");
                    }
                    else
                    {
                        item.BreakEndTime = "N/A";
                    }


                    if (item.BreakEndTime != "N/A" && item.BreakStartTime != "N/A")
                    {
                        //timesheetData.TotalRestHour = timesheetData.BreakEndTime.Hour - timesheetData.BreakStartTime.Hour;
                        item.TotalRestHour = (decimal)Math.Round((endBreakList.First().EventTime - breakList.First().EventTime).TotalMinutes / 60, 1);

                    }
                    else if (item.BreakEndTime == "N/A" && item.BreakStartTime == "N/A")
                    {
                        if (inList.Count() > 1 && outList.Count() > 1)
                        {
                            item.BreakStartTime = outList[0].EventTime.ToString("hh:mm tt");
                            item.BreakEndTime = inList[1].EventTime.ToString("hh:mm tt");

                            item.TotalRestHour = (decimal)Math.Round((inList[1].EventTime - outList[0].EventTime).TotalMinutes / 60, 1);
                        }
                    }

                    if (item.DailyEndTime != "N/A" && item.DailyStartTime != "N/A")
                    {
                        //timesheetData.TotalRestHour = timesheetData.BreakEndTime.Hour - timesheetData.BreakStartTime.Hour;                                
                        double startTime = inList.First().EventTime.TimeOfDay.Hours;

                        if (inList.First().EventTime.TimeOfDay.Minutes >= 45)
                        {
                            startTime += 1;
                        }
                        else if (inList.First().EventTime.TimeOfDay.Minutes > 15 && outList.Last().EventTime.TimeOfDay.Minutes < 45)
                        {
                            startTime += 0.5;
                        }

                        double endTime = outList.Last().EventTime.TimeOfDay.Hours;

                        if (endTime < 1)
                        {
                            endTime = 24;
                        }

                        if (outList.Last().EventTime.TimeOfDay.Minutes >= 45)
                        {
                            endTime += 1;
                        }
                        else if (outList.Last().EventTime.TimeOfDay.Minutes > 15 && outList.Last().EventTime.TimeOfDay.Minutes < 45)
                        {
                            endTime += 0.5;
                        }

                        item.TotalWorkingHour = (decimal)(endTime - startTime);
                    }
                    else
                    {
                        item.TotalWorkingHour = 0;
                    }
                }
            }

            if (Query.AttendanceStatus == "All")
            {
                foreach (var item in summary)
                {
                    if (!item.DailyStartTime.Contains("N/A"))
                    {
                        var currentHour = DateTime.Parse(item.DailyStartTime).Hour;
                        var currentMinute = DateTime.Parse(item.DailyStartTime).Minute;
                        if (currentMinute <= 30 && currentMinute >= 15)
                        {
                            item.Status = "Late";
                        }
                        else if (currentMinute > 30 && currentMinute <= 45)
                        {
                            item.Status = "Early";
                        }
                        else
                        {
                            item.Status = "On-time";
                        }
                    }
                    else
                    {
                        item.Status = "N/A";
                    }

                }
            }

            return summary;
        }

        [HttpPost("report")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public List<Models.AttendanceReport> GetTimesheetAttendanceData([FromBody] TimesheetDailyQuery Query)
        {
            List<Models.AttendanceReport> result = new List<Models.AttendanceReport>();

            List<TimesheetData> data = new List<Models.TimesheetData>();

            data = _TimesheetRepository.GetAllTimesheetData().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= Query.ToDate).ToList();

            var employees = _employeeRepository.GetEmployees();

            if (Query.Department != "All")
            {
                data = data.Where(x => x.Department == Query.Department).ToList();
                employees = employees.Where(x => x.Department == Query.Department).ToList();
            }

            if (Query.AttendanceStatus == "All")
            {
                foreach (var item in data)
                {
                    AttendanceReport record = new AttendanceReport();
                    record.Date = DateTime.Parse(item.Date);
                    record.Name = item.FirstName + " " + item.LastName;
                    record.Department = item.Department;

                    record.Present = "Yes";
                    record.ArrivalTime = item.DailyStartTime;
                    result.Add(record);
                }

                var dateList = result.Select(x => x.Date).Distinct().ToList();

                foreach (var item in dateList)
                {
                    var currentList = data.Where(x => DateTime.Parse(x.Date) == item).ToList();
                    var absentList = employees.Where(x => !currentList.Any(y => y.FirstName + " " + y.LastName == x.Name));

                    foreach (var employee in absentList)
                    {
                        AttendanceReport record = new AttendanceReport();
                        record.Date = item;
                        record.Name = employee.Name;
                        record.Department = employee.Department;

                        record.Present = "No";
                        result.Add(record);
                    }
                }
            }
            else if (Query.AttendanceStatus == "Present")
            {
                foreach (var item in data)
                {
                    AttendanceReport record = new AttendanceReport();
                    record.Date = DateTime.Parse(item.Date);
                    record.Name = item.FirstName + " " + item.LastName;
                    record.Department = item.Department;

                    record.Present = "Yes";
                    record.ArrivalTime = item.DailyStartTime;
                    result.Add(record);
                }
            }
            else
            {
                var dateList = data.Select(x => DateTime.Parse(x.Date)).Distinct().ToList();

                foreach (var item in dateList)
                {
                    var currentList = data.Where(x => DateTime.Parse(x.Date) == item).ToList();
                    var absentList = employees.Where(x => !currentList.Any(y => y.FirstName + " " + y.LastName == x.Name));

                    foreach (var employee in absentList)
                    {
                        AttendanceReport record = new AttendanceReport();
                        record.Date = item;
                        record.Name = employee.Name;
                        record.Department = employee.Department;

                        record.Present = "No";
                        result.Add(record);
                    }
                }
            }

            if (Query.AttendanceStatus != "Absent")
            {
                // Display Late/Early Status
                foreach (var item in result)
                {
                    if (item.Present != "No")
                    {
                        var currentHour = item.ArrivalTime.Hour;
                        var currentMinute = item.ArrivalTime.Minute;
                        if (currentMinute <= 30 && currentMinute >= 15)
                        {
                            item.Status = "Late";
                        }
                        else if (currentMinute > 30 && currentMinute <= 45)
                        {
                            item.Status = "Early";
                        }
                        else
                        {
                            item.Status = "On-time";
                        }
                    }
                }
            }

            return result;
        }

        [HttpPost("raw")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public List<Models.Timesheet> GetTimesheetRawData([FromBody] TimesheetDailyQuery Query)
        {
            return _TimesheetRepository.GetAllTimesheet().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= Query.ToDate).ToList();
        }

        [HttpPost("correct")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public void DeleteByDate([FromBody] TimesheetDailyQuery Query)
        {
            _TimesheetRepository.DeleteTimesheetByDateAsync(Query);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "TimesheetCorrection Deleted {Query}", Query);
        }


        // POST api/<controller>
        [HttpPost("face")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Employee_FaceReg AddFaces([FromBody] Models.Employee_FaceReg Employee_FaceReg)
        {
            if (ModelState.IsValid)
            {
                Employee_FaceReg = _TimesheetRepository.AddFaces(Employee_FaceReg);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Employee_FaceReg Added {Employee_FaceReg}", Employee_FaceReg);
            }

            var gearmentSetting = _settingRepository.GetGearmentSettings(_entityId);
            var amazonUrl = gearmentSetting.FirstOrDefault(x => x.Name == "attendance-post-url");
            if (amazonUrl != null)
            {
                var client = new RestClient(string.Format("{0}/{1}", amazonUrl.Value, Employee_FaceReg.EmployeeId));
                client.Timeout = -1;
                var request = new RestSharp.RestRequest(Method.POST);
                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
            }

            return Employee_FaceReg;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Timesheet Post([FromBody] Models.Timesheet Timesheet)
        {
            if (ModelState.IsValid && Timesheet.ModuleId == _entityId)
            {
                Timesheet = _TimesheetRepository.AddTimesheet(Timesheet);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Timesheet Added {Timesheet}", Timesheet);
            }
            return Timesheet;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.TimesheetData Put(int id, [FromBody] Models.TimesheetData Timesheet)
        {
            if (ModelState.IsValid)
            {
                Timesheet = _TimesheetRepository.UpdateTimesheet(Timesheet);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Timesheet Updated {Timesheet}", Timesheet);
            }
            return Timesheet;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.TimesheetData Timesheet = _TimesheetRepository.GetTimesheet(id);
            if (Timesheet != null)
            {
                _TimesheetRepository.DeleteTimesheet(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Timesheet Deleted {TimesheetId}", id);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("attendance/{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void DeleteEvent(int id)
        {
            var recordFound = _TimesheetRepository.GetEmployee_FaceRegEvent(id);
            if (recordFound != null)
            {
                _TimesheetRepository.DeleteFaceRegEvent(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Event Deleted {TimesheetId}", id);
            }
        }

        [HttpGet("attendance/{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.Employee_FaceRegEventDetail GetEvent(int id)
        {
            return _TimesheetRepository.GetEmployee_FaceRegEvent(id);
        }

        [HttpPut("attendance/{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Employee_FaceRegEventDetail PutEvent(int id, [FromBody] Models.Employee_FaceRegEventDetail eventDetail)
        {
            if (ModelState.IsValid)
            {
                eventDetail = _TimesheetRepository.UpdateEvent(eventDetail);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Timesheet Updated {Timesheet}", eventDetail);
            }
            return eventDetail;
        }

        [HttpPost("attendance/add")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Employee_FaceRegEvent PostEvent([FromBody] Models.Employee_FaceRegEvent Employee_FaceRegEvent)
        {
            Employee_FaceRegEvent = _TimesheetRepository.AddEvent(Employee_FaceRegEvent);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Event Added {Employee_FaceRegEvent}", Employee_FaceRegEvent);

            return Employee_FaceRegEvent;
        }

        private string ResolveApplicationPath(Oqtane.Models.File file)
        {
            file.Folder = _folders.GetFolder(file.FolderId);
            return Utilities.PathCombine(_environment.ContentRootPath, "Content", "Tenants", _tenants.GetTenant().TenantId.ToString(), "Sites", file.Folder.SiteId.ToString(), file.Folder.Path, file.Name);
        }

        private List<Models.Timesheet> ExportDataFromExcelSheet(IWorksheet sheet, int startRowIndex, int startColumnIndex, int lastRowIndex, int moduleId)
        {
            List<Models.Timesheet> result = new List<Models.Timesheet>();
            for (int r = startRowIndex; r <= lastRowIndex; r++)
            {
                Models.Timesheet record = new Models.Timesheet();
                List<string> fullName = string.IsNullOrEmpty(sheet[r, startColumnIndex].Text) ? new List<string>() : sheet[r, startColumnIndex].Text.Split(',').ToList();

                record.FirstName = fullName[1].Trim();
                record.LastName = fullName[0].Trim();

                record.ModuleId = moduleId;

                record.PayRollID = string.IsNullOrEmpty(sheet[r, startColumnIndex + 1].Value) ? string.Empty : sheet[r, startColumnIndex + 1].Value;
                record.DayOfWeek = string.IsNullOrEmpty(sheet[r, startColumnIndex + 2].Value) ? string.Empty : sheet[r, startColumnIndex + 2].Value;
                record.Date = string.IsNullOrEmpty(sheet[r, startColumnIndex + 3].Value) ? string.Empty : sheet[r, startColumnIndex + 3].Value;
                record.In = string.IsNullOrEmpty(sheet[r, startColumnIndex + 4].Value) ? string.Empty : sheet[r, startColumnIndex + 4].Value;
                record.Out = string.IsNullOrEmpty(sheet[r, startColumnIndex + 5].Value) ? string.Empty : sheet[r, startColumnIndex + 5].Value;
                record.Hours = string.IsNullOrEmpty(sheet[r, startColumnIndex + 6].Value) ? string.Empty : sheet[r, startColumnIndex + 6].Value;
                record.Type = string.IsNullOrEmpty(sheet[r, startColumnIndex + 7].Value) ? string.Empty : sheet[r, startColumnIndex + 7].Value;
                result.Add(record);
            }

            return result;
        }
    }
}
