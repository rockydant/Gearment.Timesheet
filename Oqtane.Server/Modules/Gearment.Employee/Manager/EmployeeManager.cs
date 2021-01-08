using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Gearment.Employee.Models;
using Gearment.Employee.Repository;

namespace Gearment.Employee.Manager
{
    public class EmployeeManager : IInstallable, IPortable
    {
        private IEmployeeRepository _EmployeeRepository;
        private ISqlRepository _sql;

        public EmployeeManager(IEmployeeRepository EmployeeRepository, ISqlRepository sql)
        {
            _EmployeeRepository = EmployeeRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Employee." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Employee.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.Employee> Employees = _EmployeeRepository.GetEmployees(module.ModuleId).ToList();
            if (Employees != null)
            {
                content = JsonSerializer.Serialize(Employees);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.Employee> Employees = null;
            if (!string.IsNullOrEmpty(content))
            {
                Employees = JsonSerializer.Deserialize<List<Models.Employee>>(content);
            }
            if (Employees != null)
            {
                foreach(var Employee in Employees)
                {
                    _EmployeeRepository.AddEmployee(new Models.Employee { ModuleId = module.ModuleId, Name = Employee.Name });
                }
            }
        }
    }
}