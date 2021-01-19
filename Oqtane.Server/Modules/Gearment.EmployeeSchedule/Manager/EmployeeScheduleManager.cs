using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Gearment.EmployeeSchedule.Models;
using Gearment.EmployeeSchedule.Repository;

namespace Gearment.EmployeeSchedule.Manager
{
    public class EmployeeScheduleManager : IInstallable, IPortable
    {
        private IEmployeeScheduleRepository _EmployeeScheduleRepository;
        private ISqlRepository _sql;

        public EmployeeScheduleManager(IEmployeeScheduleRepository EmployeeScheduleRepository, ISqlRepository sql)
        {
            _EmployeeScheduleRepository = EmployeeScheduleRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.EmployeeSchedule." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.EmployeeSchedule.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.EmployeeSchedule> EmployeeSchedules = _EmployeeScheduleRepository.GetEmployeeSchedules(module.ModuleId).ToList();
            if (EmployeeSchedules != null)
            {
                content = JsonSerializer.Serialize(EmployeeSchedules);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.EmployeeSchedule> EmployeeSchedules = null;
            if (!string.IsNullOrEmpty(content))
            {
                EmployeeSchedules = JsonSerializer.Deserialize<List<Models.EmployeeSchedule>>(content);
            }
            if (EmployeeSchedules != null)
            {
                foreach(var EmployeeSchedule in EmployeeSchedules)
                {
                    _EmployeeScheduleRepository.AddEmployeeSchedule(new Models.EmployeeSchedule { ModuleId = module.ModuleId, Name = EmployeeSchedule.Name });
                }
            }
        }
    }
}