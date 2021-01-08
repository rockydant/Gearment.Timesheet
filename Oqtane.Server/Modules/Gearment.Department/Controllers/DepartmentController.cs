using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Gearment.Department.Models;
using Gearment.Department.Repository;

namespace Gearment.Department.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;

        public DepartmentController(IDepartmentRepository DepartmentRepository, ILogManager logger, IHttpContextAccessor accessor)
        {
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
        public IEnumerable<Models.Department> Get(string moduleid)
        {
            return _DepartmentRepository.GetDepartments(int.Parse(moduleid));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.Department Get(int id)
        {
            Models.Department Department = _DepartmentRepository.GetDepartment(id);
            if (Department != null && Department.ModuleId != _entityId)
            {
                Department = null;
            }
            return Department;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Department Post([FromBody] Models.Department Department)
        {
            if (ModelState.IsValid && Department.ModuleId == _entityId)
            {
                Department = _DepartmentRepository.AddDepartment(Department);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Department Added {Department}", Department);
            }
            return Department;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Department Put(int id, [FromBody] Models.Department Department)
        {
            if (ModelState.IsValid && Department.ModuleId == _entityId)
            {
                Department = _DepartmentRepository.UpdateDepartment(Department);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Department Updated {Department}", Department);
            }
            return Department;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.Department Department = _DepartmentRepository.GetDepartment(id);
            if (Department != null && Department.ModuleId == _entityId)
            {
                _DepartmentRepository.DeleteDepartment(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Department Deleted {DepartmentId}", id);
            }
        }
    }
}
