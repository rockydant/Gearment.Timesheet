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
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Gearment.Employee.Repository;
using Gearment.Department.Repository;
using System;

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

        public TimesheetController(ITimesheetRepository TimesheetRepository, IEmployeeRepository EmployeeRepository, IFolderRepository Folders, IDepartmentRepository DepartmentRepository, IWebHostEnvironment environment, ITenantResolver tenants, IUserPermissions userPermissions, IFileRepository files, ILogManager logger, IHttpContextAccessor accessor)
        {
            _TimesheetRepository = TimesheetRepository;
            _logger = logger;
            _files = files;
            _userPermissions = userPermissions;
            _environment = environment;
            _tenants = tenants;
            _employeeRepository = EmployeeRepository;
            _departmentRepository = DepartmentRepository;
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
                                timesheetData.TotalRestHour = (int)Math.Round((timesheetData.BreakEndTime - timesheetData.BreakStartTime).TotalMinutes / 60);
                            }
                            else
                            {
                                timesheetData.TotalRestHour = 0;
                            }

                            if (timesheetData.DailyEndTime != null || timesheetData.DailyStartTime != null)
                            {
                                //timesheetData.TotalRestHour = timesheetData.BreakEndTime.Hour - timesheetData.BreakStartTime.Hour;
                                timesheetData.TotalWorkingHour = (int)Math.Round((timesheetData.DailyEndTime - timesheetData.DailyStartTime).TotalMinutes / 60);
                            }
                            else
                            {
                                timesheetData.TotalWorkingHour = 0;
                            }

                            timesheetData.Status = employee.Status;

                            timesheetData.CreatedBy = User.Identity.Name;
                            timesheetData.CreatedOn = DateTime.Now;
                            timesheetData.ModifiedBy = User.Identity.Name;
                            timesheetData.ModifiedOn = DateTime.Now;
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

        [HttpPost("attendance")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public List<Models.AttendanceReport> GetTimesheetAttendanceData([FromBody] TimesheetDailyQuery Query)
        {
            List<Models.AttendanceReport> result = new List<Models.AttendanceReport>();

            List<TimesheetData> data = new List<Models.TimesheetData>();

            data = _TimesheetRepository.GetAllTimesheetData().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= DateTime.Parse(x.Date)).ToList();

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
