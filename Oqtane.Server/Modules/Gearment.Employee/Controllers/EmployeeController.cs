using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Gearment.Employee.Models;
using Gearment.Department.Models;
using Gearment.Employee.Repository;
using Gearment.Department.Repository;
using Oqtane.Repository;
using Oqtane.Security;
using Syncfusion.XlsIO;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Linq;
using System;

namespace Gearment.Employee.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly IFileRepository _files;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly ITenantResolver _tenants;
        private readonly IWebHostEnvironment _environment;

        protected int _entityId = -1;

        public EmployeeController(IEmployeeRepository EmployeeRepository, IDepartmentRepository DepartmentRepository, ITenantResolver tenants, IWebHostEnvironment environment, IUserPermissions userPermissions, IFileRepository files, ILogManager logger, IHttpContextAccessor accessor)
        {
            _EmployeeRepository = EmployeeRepository;
            _DepartmentRepository = DepartmentRepository;
            _files = files;
            _userPermissions = userPermissions;
            _environment = environment;
            _logger = logger;
            _tenants = tenants;

            if (accessor.HttpContext.Request.Query.ContainsKey("entityid"))
            {
                _entityId = int.Parse(accessor.HttpContext.Request.Query["entityid"]);
            }
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.Employee> Get(string moduleid)
        {
            return _EmployeeRepository.GetEmployees(int.Parse(moduleid));
        }

        [HttpGet("all")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.Employee> GetAll()
        {
            return _EmployeeRepository.GetEmployees();
        }

        [HttpGet("departments")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Gearment.Department.Models.DepartmentViewModel> GetDepartment()
        {
            return _DepartmentRepository.GetDepartments();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.Employee Get(int id)
        {
            Models.Employee Employee = _EmployeeRepository.GetEmployee(id);
            if (Employee != null && Employee.ModuleId != _entityId)
            {
                Employee = null;
            }
            return Employee;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Employee Post([FromBody] Models.Employee Employee)
        {
            if (ModelState.IsValid && Employee.ModuleId == _entityId)
            {
                Employee = _EmployeeRepository.AddEmployee(Employee);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Employee Added {Employee}", Employee);
            }
            return Employee;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Employee Put(int id, [FromBody] Models.Employee Employee)
        {
            if (ModelState.IsValid && Employee.ModuleId == _entityId)
            {
                Employee = _EmployeeRepository.UpdateEmployee(Employee);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Employee Updated {Employee}", Employee);
            }
            return Employee;
        }

        [HttpPut("update")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public IEnumerable<Models.Employee> PutEmployees([FromBody] IEnumerable<Models.Employee> Employees)
        {
            if (ModelState.IsValid)
            {
                Employees = _EmployeeRepository.UpdateEmployees(Employees);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Employee Updated {Employees}", Employees);
            }

            return Employees;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.Employee Employee = _EmployeeRepository.GetEmployee(id);
            if (Employee != null && Employee.ModuleId == _entityId)
            {
                _EmployeeRepository.DeleteEmployee(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Employee Deleted {EmployeeId}", id);
            }
        }

        [HttpGet("process/{moduleId}/{id}")]
        public IEnumerable<Models.Employee> Process(int moduleId, int id)
        {
            Oqtane.Models.File file = _files.GetFile(id);
            List<Models.Employee> importList = new List<Models.Employee>();
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
                        importList = ExportDataFromExcelSheet(worksheet, 2, 1, dataTable.Rows.Count + 1, moduleId);
                    }

                    List<string> foundDepartmentList = importList.Select(x => x.Department).Distinct().ToList();
                    foreach (var item in foundDepartmentList)
                    {
                        var record = _DepartmentRepository.GetDepartmentByName(item);
                        if (record == null)
                        {
                            Department.Models.Department department = new Department.Models.Department();
                            department.ModuleId = moduleId;
                            department.Name = item;
                            department.DailyStartTime = DateTime.UtcNow;
                            department.DailyEndTime = DateTime.UtcNow;
                            department.BreakStartTime = DateTime.UtcNow;
                            department.BreakEndTime = DateTime.UtcNow;
                            department.TotalRestHour = 0;

                            _DepartmentRepository.AddDepartment(department);
                        }
                    }

                    foreach (var item in importList)
                    {
                        var row = _EmployeeRepository.GetEmployeeByNameOrPayrollId(item);
                        if (row != null)
                        {
                            var foundRow = _EmployeeRepository.GetEmployee(row.FirstOrDefault().EmployeeId);
                            foundRow.Rate = item.Rate;
                            foundRow.Department = item.Department;
                        }
                        else
                        {
                            Models.Employee missing = new Models.Employee();
                            missing.Name = item.Name;
                            missing.PayrollID = 0;
                            missing.Rate = item.Rate;
                            missing.Department = item.Department;
                            missing.StartDate = DateTime.UtcNow;
                            missing.Status = "Active";
                            missing.Note = string.Empty;
                            missing.ModuleId = moduleId;
                        }
                    }

                    return importList;
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

        private string ResolveApplicationPath(Oqtane.Models.File file)
        {
            return Utilities.PathCombine(_environment.ContentRootPath, "Content", "Tenants", _tenants.GetTenant().TenantId.ToString(), "Sites", file.FolderId.ToString(), file.Name);
        }

        private List<Models.Employee> ExportDataFromExcelSheet(IWorksheet sheet, int startRowIndex, int startColumnIndex, int lastRowIndex, int moduleId)
        {
            List<Models.Employee> result = new List<Models.Employee>();
            for (int r = startRowIndex; r <= lastRowIndex; r++)
            {
                Models.Employee record = new Models.Employee();
                List<string> fullName = string.IsNullOrEmpty(sheet[r, startColumnIndex].Text) ? new List<string>() : sheet[r, startColumnIndex].Text.Split(" ").ToList();

                record.Name = fullName[1].Trim() + "," + fullName[0].Trim();

                record.ModuleId = moduleId;

                record.Department = string.IsNullOrEmpty(sheet[r, startColumnIndex + 1].Text) ? string.Empty : sheet[r, startColumnIndex + 1].Text;
                record.Rate = string.IsNullOrEmpty(sheet[r, startColumnIndex + 2].Text) ? 0.0 : double.Parse(sheet[r, startColumnIndex + 2].Text.Replace("$", string.Empty).Trim());

                result.Add(record);
            }

            return result;
        }
    }
}
