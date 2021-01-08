using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Gearment.Department.Models;
using Gearment.Department.Repository;

namespace Gearment.Department.Manager
{
    public class DepartmentManager : IInstallable, IPortable
    {
        private IDepartmentRepository _DepartmentRepository;
        private ISqlRepository _sql;

        public DepartmentManager(IDepartmentRepository DepartmentRepository, ISqlRepository sql)
        {
            _DepartmentRepository = DepartmentRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Department." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Department.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.Department> Departments = _DepartmentRepository.GetDepartments(module.ModuleId).ToList();
            if (Departments != null)
            {
                content = JsonSerializer.Serialize(Departments);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.Department> Departments = null;
            if (!string.IsNullOrEmpty(content))
            {
                Departments = JsonSerializer.Deserialize<List<Models.Department>>(content);
            }
            if (Departments != null)
            {
                foreach(var Department in Departments)
                {
                    _DepartmentRepository.AddDepartment(new Models.Department { ModuleId = module.ModuleId, Name = Department.Name });
                }
            }
        }
    }
}