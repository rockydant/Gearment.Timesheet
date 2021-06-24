using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Gearment.GearmentSetting.Models;
using Gearment.GearmentSetting.Repository;

namespace Gearment.GearmentSetting.Manager
{
    public class GearmentSettingManager : IInstallable, IPortable
    {
        private IGearmentSettingRepository _GearmentSettingRepository;
        private ISqlRepository _sql;

        public GearmentSettingManager(IGearmentSettingRepository GearmentSettingRepository, ISqlRepository sql)
        {
            _GearmentSettingRepository = GearmentSettingRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.GearmentSetting." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.GearmentSetting.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.GearmentSetting> GearmentSettings = _GearmentSettingRepository.GetGearmentSettings(module.ModuleId).ToList();
            if (GearmentSettings != null)
            {
                content = JsonSerializer.Serialize(GearmentSettings);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.GearmentSetting> GearmentSettings = null;
            if (!string.IsNullOrEmpty(content))
            {
                GearmentSettings = JsonSerializer.Deserialize<List<Models.GearmentSetting>>(content);
            }
            if (GearmentSettings != null)
            {
                foreach(var GearmentSetting in GearmentSettings)
                {
                    _GearmentSettingRepository.AddGearmentSetting(new Models.GearmentSetting { ModuleId = module.ModuleId, Name = GearmentSetting.Name });
                }
            }
        }
    }
}