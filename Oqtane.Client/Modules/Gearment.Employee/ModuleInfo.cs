using Oqtane.Models;
using Oqtane.Modules;

namespace Gearment.Employee
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Employee",
            Description = "Employee",
            Version = "1.0.1",
            ServerManagerType = "Gearment.Employee.Manager.EmployeeManager, Oqtane.Server",
            ReleaseVersions = "1.0.1"
        };
    }
}
