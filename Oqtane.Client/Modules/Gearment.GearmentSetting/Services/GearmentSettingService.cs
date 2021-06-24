using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Gearment.GearmentSetting.Models;

namespace Gearment.GearmentSetting.Services
{
    public class GearmentSettingService : ServiceBase, IGearmentSettingService, IService
    {
        private readonly SiteState _siteState;

        public GearmentSettingService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

         private string Apiurl => CreateApiUrl(_siteState.Alias, "GearmentSetting");

        public async Task<List<Models.GearmentSetting>> GetGearmentSettingsAsync(int ModuleId)
        {
            List<Models.GearmentSetting> GearmentSettings = await GetJsonAsync<List<Models.GearmentSetting>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", ModuleId));
            return GearmentSettings.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.GearmentSetting> GetGearmentSettingAsync(int GearmentSettingId, int ModuleId)
        {
            return await GetJsonAsync<Models.GearmentSetting>(CreateAuthorizationPolicyUrl($"{Apiurl}/{GearmentSettingId}", ModuleId));
        }

        public async Task<Models.GearmentSetting> AddGearmentSettingAsync(Models.GearmentSetting GearmentSetting)
        {
            return await PostJsonAsync<Models.GearmentSetting>(CreateAuthorizationPolicyUrl($"{Apiurl}", GearmentSetting.ModuleId), GearmentSetting);
        }

        public async Task<Models.GearmentSetting> UpdateGearmentSettingAsync(Models.GearmentSetting GearmentSetting)
        {
            return await PutJsonAsync<Models.GearmentSetting>(CreateAuthorizationPolicyUrl($"{Apiurl}/{GearmentSetting.GearmentSettingId}", GearmentSetting.ModuleId), GearmentSetting);
        }

        public async Task DeleteGearmentSettingAsync(int GearmentSettingId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{GearmentSettingId}", ModuleId));
        }
    }
}
