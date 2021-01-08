using Oqtane.Models;
using Oqtane.Modules;

namespace Gearment.Department
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Department",
            Description = "Department",
            Version = "1.0.0",
            ServerManagerType = "Gearment.Department.Manager.DepartmentManager, Oqtane.Server",
            ReleaseVersions = "1.0.0"
        };
    }
}
