using Oqtane.Models;
using Oqtane.Modules;

namespace Gearment.GearmentSetting
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "GearmentSetting",
            Description = "GearmentSetting",
            Version = "1.0.0",
            ServerManagerType = "Gearment.GearmentSetting.Manager.GearmentSettingManager, Oqtane.Server",
            ReleaseVersions = "1.0.0"
        };
    }
}
