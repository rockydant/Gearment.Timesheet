using System.Collections.Generic;
using System.Threading.Tasks;
using Gearment.GearmentSetting.Models;

namespace Gearment.GearmentSetting.Services
{
    public interface IGearmentSettingService 
    {
        Task<List<Models.GearmentSetting>> GetGearmentSettingsAsync(int ModuleId);

        Task<Models.GearmentSetting> GetGearmentSettingAsync(int GearmentSettingId, int ModuleId);

        Task<Models.GearmentSetting> AddGearmentSettingAsync(Models.GearmentSetting GearmentSetting);

        Task<Models.GearmentSetting> UpdateGearmentSettingAsync(Models.GearmentSetting GearmentSetting);

        Task DeleteGearmentSettingAsync(int GearmentSettingId, int ModuleId);
    }
}
