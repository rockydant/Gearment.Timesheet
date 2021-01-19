using Oqtane.Models;
using Oqtane.Modules;

namespace Gearment.EmployeeSchedule
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "EmployeeSchedule",
            Description = "EmployeeSchedule",
            Version = "1.0.0",
            ServerManagerType = "Gearment.EmployeeSchedule.Manager.EmployeeScheduleManager, Oqtane.Server",
            ReleaseVersions = "1.0.0"
        };
    }
}
