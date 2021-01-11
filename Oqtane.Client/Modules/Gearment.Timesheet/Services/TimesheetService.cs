using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Services
{
    public class TimesheetService : ServiceBase, ITimesheetService, IService
    {
        private readonly SiteState _siteState;

        public TimesheetService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

         private string Apiurl => CreateApiUrl(_siteState.Alias, "Timesheet");

        public async Task<List<Models.Timesheet>> GetTimesheetsAsync(int ModuleId)
        {
            List<Models.Timesheet> Timesheets = await GetJsonAsync<List<Models.Timesheet>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", ModuleId));
            return Timesheets.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Timesheet> GetTimesheetAsync(int TimesheetId, int ModuleId)
        {
            return await GetJsonAsync<Models.Timesheet>(CreateAuthorizationPolicyUrl($"{Apiurl}/{TimesheetId}", ModuleId));
        }

        public async Task<Models.Timesheet> AddTimesheetAsync(Models.Timesheet Timesheet)
        {
            return await PostJsonAsync<Models.Timesheet>(CreateAuthorizationPolicyUrl($"{Apiurl}", Timesheet.ModuleId), Timesheet);
        }

        public async Task<Models.Timesheet> UpdateTimesheetAsync(Models.Timesheet Timesheet)
        {
            return await PutJsonAsync<Models.Timesheet>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Timesheet.TimesheetId}", Timesheet.ModuleId), Timesheet);
        }

        public async Task DeleteTimesheetAsync(int TimesheetId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{TimesheetId}", ModuleId));
        }
    }
}
