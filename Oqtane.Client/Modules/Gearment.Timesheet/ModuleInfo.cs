using Oqtane.Models;
using Oqtane.Modules;

namespace Gearment.Timesheet
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Timesheet",
            Description = "Timesheet",
            Version = "1.0.4",
            ServerManagerType = "Gearment.Timesheet.Manager.TimesheetManager, Oqtane.Server",
            ReleaseVersions = "1.0.4"
        };
    }
}
