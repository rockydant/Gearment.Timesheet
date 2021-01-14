using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Gearment.Summary.Models;
using Gearment.Summary.Repository;

namespace Gearment.Summary.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class SummaryController : Controller
    {
        private readonly ISummaryRepository _SummaryRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;

        public SummaryController(ISummaryRepository SummaryRepository, ILogManager logger, IHttpContextAccessor accessor)
        {
            _SummaryRepository = SummaryRepository;
            _logger = logger;

            if (accessor.HttpContext.Request.Query.ContainsKey("entityid"))
            {
                _entityId = int.Parse(accessor.HttpContext.Request.Query["entityid"]);
            }
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.Summary> Get(string moduleid)
        {
            return _SummaryRepository.GetSummarys(int.Parse(moduleid));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.Summary Get(int id)
        {
            Models.Summary Summary = _SummaryRepository.GetSummary(id);
            if (Summary != null && Summary.ModuleId != _entityId)
            {
                Summary = null;
            }
            return Summary;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Summary Post([FromBody] Models.Summary Summary)
        {
            if (ModelState.IsValid && Summary.ModuleId == _entityId)
            {
                Summary = _SummaryRepository.AddSummary(Summary);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Summary Added {Summary}", Summary);
            }
            return Summary;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Summary Put(int id, [FromBody] Models.Summary Summary)
        {
            if (ModelState.IsValid && Summary.ModuleId == _entityId)
            {
                Summary = _SummaryRepository.UpdateSummary(Summary);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Summary Updated {Summary}", Summary);
            }
            return Summary;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.Summary Summary = _SummaryRepository.GetSummary(id);
            if (Summary != null && Summary.ModuleId == _entityId)
            {
                _SummaryRepository.DeleteSummary(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Summary Deleted {SummaryId}", id);
            }
        }
    }
}
