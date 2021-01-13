using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Gearment.Timesheet.Models;
using Gearment.Timesheet.Repository;

namespace Gearment.Timesheet.Manager
{
    public class TimesheetManager : IInstallable, IPortable
    {
        private ITimesheetRepository _TimesheetRepository;
        private ISqlRepository _sql;

        public TimesheetManager(ITimesheetRepository TimesheetRepository, ISqlRepository sql)
        {
            _TimesheetRepository = TimesheetRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Timesheet." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Timesheet.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.Timesheet> Timesheets = _TimesheetRepository.GetTimesheets(module.ModuleId).ToList();
            if (Timesheets != null)
            {
                content = JsonSerializer.Serialize(Timesheets);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.Timesheet> Timesheets = null;
            if (!string.IsNullOrEmpty(content))
            {
                Timesheets = JsonSerializer.Deserialize<List<Models.Timesheet>>(content);
            }
            if (Timesheets != null)
            {
                foreach(var Timesheet in Timesheets)
                {
                    _TimesheetRepository.AddTimesheet(new Models.Timesheet { ModuleId = module.ModuleId, FirstName = Timesheet.FirstName });
                }
            }
        }
    }
}
