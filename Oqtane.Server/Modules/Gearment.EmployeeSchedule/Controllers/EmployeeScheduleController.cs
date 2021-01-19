using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Gearment.EmployeeSchedule.Models;
using Gearment.EmployeeSchedule.Repository;

namespace Gearment.EmployeeSchedule.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class EmployeeScheduleController : Controller
    {
        private readonly IEmployeeScheduleRepository _EmployeeScheduleRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;

        public EmployeeScheduleController(IEmployeeScheduleRepository EmployeeScheduleRepository, ILogManager logger, IHttpContextAccessor accessor)
        {
            _EmployeeScheduleRepository = EmployeeScheduleRepository;
            _logger = logger;

            if (accessor.HttpContext.Request.Query.ContainsKey("entityid"))
            {
                _entityId = int.Parse(accessor.HttpContext.Request.Query["entityid"]);
            }
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.EmployeeSchedule> Get(string moduleid)
        {
            return _EmployeeScheduleRepository.GetEmployeeSchedules(int.Parse(moduleid));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.EmployeeSchedule Get(int id)
        {
            Models.EmployeeSchedule EmployeeSchedule = _EmployeeScheduleRepository.GetEmployeeSchedule(id);
            if (EmployeeSchedule != null && EmployeeSchedule.ModuleId != _entityId)
            {
                EmployeeSchedule = null;
            }
            return EmployeeSchedule;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.EmployeeSchedule Post([FromBody] Models.EmployeeSchedule EmployeeSchedule)
        {
            if (ModelState.IsValid && EmployeeSchedule.ModuleId == _entityId)
            {
                EmployeeSchedule = _EmployeeScheduleRepository.AddEmployeeSchedule(EmployeeSchedule);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "EmployeeSchedule Added {EmployeeSchedule}", EmployeeSchedule);
            }
            return EmployeeSchedule;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.EmployeeSchedule Put(int id, [FromBody] Models.EmployeeSchedule EmployeeSchedule)
        {
            if (ModelState.IsValid && EmployeeSchedule.ModuleId == _entityId)
            {
                EmployeeSchedule = _EmployeeScheduleRepository.UpdateEmployeeSchedule(EmployeeSchedule);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "EmployeeSchedule Updated {EmployeeSchedule}", EmployeeSchedule);
            }
            return EmployeeSchedule;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.EmployeeSchedule EmployeeSchedule = _EmployeeScheduleRepository.GetEmployeeSchedule(id);
            if (EmployeeSchedule != null && EmployeeSchedule.ModuleId == _entityId)
            {
                _EmployeeScheduleRepository.DeleteEmployeeSchedule(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "EmployeeSchedule Deleted {EmployeeScheduleId}", id);
            }
        }
    }
}
