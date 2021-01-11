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

namespace Gearment.Employee.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;

        public EmployeeController(IEmployeeRepository EmployeeRepository, IDepartmentRepository DepartmentRepository, ILogManager logger, IHttpContextAccessor accessor)
        {
            _EmployeeRepository = EmployeeRepository;
            _DepartmentRepository = DepartmentRepository;
            _logger = logger;

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
    }
}
