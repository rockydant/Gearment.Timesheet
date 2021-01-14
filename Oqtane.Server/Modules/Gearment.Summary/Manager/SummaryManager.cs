using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Gearment.Summary.Models;
using Gearment.Summary.Repository;

namespace Gearment.Summary.Manager
{
    public class SummaryManager : IInstallable, IPortable
    {
        private ISummaryRepository _SummaryRepository;
        private ISqlRepository _sql;

        public SummaryManager(ISummaryRepository SummaryRepository, ISqlRepository sql)
        {
            _SummaryRepository = SummaryRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Summary." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Gearment.Summary.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.Summary> Summarys = _SummaryRepository.GetSummarys(module.ModuleId).ToList();
            if (Summarys != null)
            {
                content = JsonSerializer.Serialize(Summarys);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.Summary> Summarys = null;
            if (!string.IsNullOrEmpty(content))
            {
                Summarys = JsonSerializer.Deserialize<List<Models.Summary>>(content);
            }
            if (Summarys != null)
            {
                foreach(var Summary in Summarys)
                {
                    _SummaryRepository.AddSummary(new Models.Summary { ModuleId = module.ModuleId, Name = Summary.Name });
                }
            }
        }
    }
}