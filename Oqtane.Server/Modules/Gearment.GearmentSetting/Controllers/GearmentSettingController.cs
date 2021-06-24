using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Gearment.GearmentSetting.Models;
using Gearment.GearmentSetting.Repository;

namespace Gearment.GearmentSetting.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class GearmentSettingController : Controller
    {
        private readonly IGearmentSettingRepository _GearmentSettingRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;

        public GearmentSettingController(IGearmentSettingRepository GearmentSettingRepository, ILogManager logger, IHttpContextAccessor accessor)
        {
            _GearmentSettingRepository = GearmentSettingRepository;
            _logger = logger;

            if (accessor.HttpContext.Request.Query.ContainsKey("entityid"))
            {
                _entityId = int.Parse(accessor.HttpContext.Request.Query["entityid"]);
            }
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.GearmentSetting> Get(string moduleid)
        {
            return _GearmentSettingRepository.GetGearmentSettings(int.Parse(moduleid));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.GearmentSetting Get(int id)
        {
            Models.GearmentSetting GearmentSetting = _GearmentSettingRepository.GetGearmentSetting(id);
            if (GearmentSetting != null && GearmentSetting.ModuleId != _entityId)
            {
                GearmentSetting = null;
            }
            return GearmentSetting;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.GearmentSetting Post([FromBody] Models.GearmentSetting GearmentSetting)
        {
            if (ModelState.IsValid && GearmentSetting.ModuleId == _entityId)
            {
                GearmentSetting = _GearmentSettingRepository.AddGearmentSetting(GearmentSetting);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "GearmentSetting Added {GearmentSetting}", GearmentSetting);
            }
            return GearmentSetting;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.GearmentSetting Put(int id, [FromBody] Models.GearmentSetting GearmentSetting)
        {
            if (ModelState.IsValid && GearmentSetting.ModuleId == _entityId)
            {
                GearmentSetting = _GearmentSettingRepository.UpdateGearmentSetting(GearmentSetting);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "GearmentSetting Updated {GearmentSetting}", GearmentSetting);
            }
            return GearmentSetting;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.GearmentSetting GearmentSetting = _GearmentSettingRepository.GetGearmentSetting(id);
            if (GearmentSetting != null && GearmentSetting.ModuleId == _entityId)
            {
                _GearmentSettingRepository.DeleteGearmentSetting(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "GearmentSetting Deleted {GearmentSettingId}", id);
            }
        }
    }
}
