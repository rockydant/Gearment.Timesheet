using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Gearment.Summary.Models;

namespace Gearment.Summary.Services
{
    public class SummaryService : ServiceBase, ISummaryService, IService
    {
        private readonly SiteState _siteState;

        public SummaryService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

         private string Apiurl => CreateApiUrl(_siteState.Alias, "Summary");

        public async Task<List<Models.Summary>> GetSummarysAsync(int ModuleId)
        {
            List<Models.Summary> Summarys = await GetJsonAsync<List<Models.Summary>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", ModuleId));
            return Summarys.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Summary> GetSummaryAsync(int SummaryId, int ModuleId)
        {
            return await GetJsonAsync<Models.Summary>(CreateAuthorizationPolicyUrl($"{Apiurl}/{SummaryId}", ModuleId));
        }

        public async Task<Models.Summary> AddSummaryAsync(Models.Summary Summary)
        {
            return await PostJsonAsync<Models.Summary>(CreateAuthorizationPolicyUrl($"{Apiurl}", Summary.ModuleId), Summary);
        }

        public async Task<Models.Summary> UpdateSummaryAsync(Models.Summary Summary)
        {
            return await PutJsonAsync<Models.Summary>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Summary.SummaryId}", Summary.ModuleId), Summary);
        }

        public async Task DeleteSummaryAsync(int SummaryId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{SummaryId}", ModuleId));
        }
    }
}
