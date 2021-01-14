using Oqtane.Models;
using Oqtane.Modules;

namespace Gearment.Summary
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Summary",
            Description = "Summary",
            Version = "1.0.0",
            ServerManagerType = "Gearment.Summary.Manager.SummaryManager, Oqtane.Server",
            ReleaseVersions = "1.0.0"
        };
    }
}
