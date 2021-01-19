using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Gearment.EmployeeSchedule.Models;

namespace Gearment.EmployeeSchedule.Services
{
    public class EmployeeScheduleService : ServiceBase, IEmployeeScheduleService, IService
    {
        private readonly SiteState _siteState;

        public EmployeeScheduleService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

         private string Apiurl => CreateApiUrl(_siteState.Alias, "EmployeeSchedule");

        public async Task<List<Models.EmployeeSchedule>> GetEmployeeSchedulesAsync(int ModuleId)
        {
            List<Models.EmployeeSchedule> EmployeeSchedules = await GetJsonAsync<List<Models.EmployeeSchedule>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", ModuleId));
            return EmployeeSchedules.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.EmployeeSchedule> GetEmployeeScheduleAsync(int EmployeeScheduleId, int ModuleId)
        {
            return await GetJsonAsync<Models.EmployeeSchedule>(CreateAuthorizationPolicyUrl($"{Apiurl}/{EmployeeScheduleId}", ModuleId));
        }

        public async Task<Models.EmployeeSchedule> AddEmployeeScheduleAsync(Models.EmployeeSchedule EmployeeSchedule)
        {
            return await PostJsonAsync<Models.EmployeeSchedule>(CreateAuthorizationPolicyUrl($"{Apiurl}", EmployeeSchedule.ModuleId), EmployeeSchedule);
        }

        public async Task<Models.EmployeeSchedule> UpdateEmployeeScheduleAsync(Models.EmployeeSchedule EmployeeSchedule)
        {
            return await PutJsonAsync<Models.EmployeeSchedule>(CreateAuthorizationPolicyUrl($"{Apiurl}/{EmployeeSchedule.EmployeeScheduleId}", EmployeeSchedule.ModuleId), EmployeeSchedule);
        }

        public async Task DeleteEmployeeScheduleAsync(int EmployeeScheduleId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{EmployeeScheduleId}", ModuleId));
        }
    }
}
