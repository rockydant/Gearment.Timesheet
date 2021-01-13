using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Models;
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
using Gearment.Employee.Models;
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
        private readonly IUserPermissions _userPermissions;
        private readonly IWebHostEnvironment _environment;
        private readonly ITenantResolver _tenants;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public TimesheetController(ITimesheetRepository TimesheetRepository, IEmployeeRepository EmployeeRepository, IDepartmentRepository DepartmentRepository, IWebHostEnvironment environment, ITenantResolver tenants, IUserPermissions userPermissions, IFileRepository files, ILogManager logger, IHttpContextAccessor accessor)
        {
            _TimesheetRepository = TimesheetRepository;
            _logger = logger;
            _files = files;
            _userPermissions = userPermissions;
            _environment = environment;
            _tenants = tenants;
            _employeeRepository = EmployeeRepository;
            _departmentRepository = DepartmentRepository;

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

                    foreach (var item in timesheetList)
                    {
                        var row = _TimesheetRepository.AddTimesheet(item);
                        if (row != null)
                        {
                            result.Add(row);
                        }
                    }

                    if (result.Any())
                    {
                        List<TimesheetDataViewModel> dataViewModel = new List<TimesheetDataViewModel>();

                        foreach (var item in result)
                        {
                            var foundRecord = dataViewModel.FirstOrDefault(x => x.FirstName == item.FirstName && x.LastName == item.LastName && x.PayrollID == item.PayRollID && x.Date == item.Date);
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

                        foreach (var item in dataViewModel)
                        {
                            TimesheetData timesheetData = new TimesheetData();

                            Employee.Models.Employee employee = new Employee.Models.Employee();
                            employee.Name = item.LastName + "," + item.FirstName;

                            if (!string.IsNullOrEmpty(item.PayrollID))
                            {
                                employee.PayrollID = int.Parse(item.PayrollID);
                            }

                            employee = _employeeRepository.GetEmployeeByNameOrPayrollId(employee).FirstOrDefault();

                            if (employee != null)
                            {
                                timesheetData.FirstName = item.FirstName;
                                timesheetData.LastName = item.LastName;

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
                                    timesheetData.TotalRestHour = 0;
                                }
                                else
                                {
                                    timesheetData.TotalRestHour = 0;
                                }

                                timesheetData.Status = employee.Status;
                                _TimesheetRepository.AddTimesheetData(timesheetData);
                            }
                            else
                            {
                                Employee.Models.Employee missing = new Employee.Models.Employee()
                                {
                                    Name = item.FirstName + "," + item.LastName,
                                    PayrollID = int.Parse(item.PayrollID),
                                    Rate = -1,
                                    Department = string.Empty,
                                    StartDate = DateTime.UtcNow,
                                    Status = "Active",
                                    Note = string.Empty,
                                    ModuleId = moduleId
                                };

                                if (!missingEmployeeList.Any(x => x.Name == missing.Name && x.PayrollID == missing.PayrollID))
                                {
                                    missingEmployeeList.Add(missing);
                                    _employeeRepository.AddEmployee(missing);
                                }
                            }
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
        public Models.Timesheet Get(int id)
        {
            Models.Timesheet Timesheet = _TimesheetRepository.GetTimesheet(id);
            if (Timesheet != null && Timesheet.ModuleId != _entityId)
            {
                Timesheet = null;
            }
            return Timesheet;
        }

        [HttpGet("data")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public List<Models.TimesheetData> GetTimesheetData()
        {
            return _TimesheetRepository.GetAllTimesheetData();
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
        public Models.Timesheet Put(int id, [FromBody] Models.Timesheet Timesheet)
        {
            if (ModelState.IsValid && Timesheet.ModuleId == _entityId)
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
            Models.Timesheet Timesheet = _TimesheetRepository.GetTimesheet(id);
            if (Timesheet != null && Timesheet.ModuleId == _entityId)
            {
                _TimesheetRepository.DeleteTimesheet(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Timesheet Deleted {TimesheetId}", id);
            }
        }

        private string ResolveApplicationPath(Oqtane.Models.File file)
        {
            return Utilities.PathCombine(_environment.ContentRootPath, "Content", "Tenants", _tenants.GetTenant().TenantId.ToString(), "Sites", file.FolderId.ToString(), file.Name);
        }

        private List<Models.Timesheet> ExportDataFromExcelSheet(IWorksheet sheet, int startRowIndex, int startColumnIndex, int lastRowIndex, int moduleId)
        {
            List<Models.Timesheet> result = new List<Models.Timesheet>();
            for (int r = startRowIndex; r <= lastRowIndex; r++)
            {
                Models.Timesheet record = new Models.Timesheet();
                List<string> fullName = string.IsNullOrEmpty(sheet[r, startColumnIndex].Text) ? new List<string>() : sheet[r, startColumnIndex].Text.Split(',').ToList();

                record.FirstName = fullName[1];
                record.LastName = fullName[0];

                record.ModuleId = moduleId;

                record.PayRollID = string.IsNullOrEmpty(sheet[r, startColumnIndex + 1].Text) ? string.Empty : sheet[r, startColumnIndex + 1].Text;
                record.DayOfWeek = string.IsNullOrEmpty(sheet[r, startColumnIndex + 2].Text) ? string.Empty : sheet[r, startColumnIndex + 2].Text;
                record.Date = string.IsNullOrEmpty(sheet[r, startColumnIndex + 3].Text) ? string.Empty : sheet[r, startColumnIndex + 3].Text;
                record.In = string.IsNullOrEmpty(sheet[r, startColumnIndex + 4].Text) ? string.Empty : sheet[r, startColumnIndex + 4].Text;
                record.Out = string.IsNullOrEmpty(sheet[r, startColumnIndex + 5].Text) ? string.Empty : sheet[r, startColumnIndex + 5].Text;
                record.Hours = string.IsNullOrEmpty(sheet[r, startColumnIndex + 6].Text) ? string.Empty : sheet[r, startColumnIndex + 6].Text;
                record.Type = string.IsNullOrEmpty(sheet[r, startColumnIndex + 7].Text) ? string.Empty : sheet[r, startColumnIndex + 7].Text;
                result.Add(record);
            }

            return result;
        }
    }
}
