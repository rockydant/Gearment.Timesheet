using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Gearment.Timesheet.Models;
using Gearment.Timesheet.Repository;

namespace Gearment.Timesheet.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _TimesheetRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;

        public TimesheetController(ITimesheetRepository TimesheetRepository, ILogManager logger, IHttpContextAccessor accessor)
        {
            _TimesheetRepository = TimesheetRepository;
            _logger = logger;

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
    }
}
