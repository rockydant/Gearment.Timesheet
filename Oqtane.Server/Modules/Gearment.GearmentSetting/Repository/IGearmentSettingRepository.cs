using System.Collections.Generic;
using Gearment.GearmentSetting.Models;

namespace Gearment.GearmentSetting.Repository
{
    public interface IGearmentSettingRepository
    {
        IEnumerable<Models.GearmentSetting> GetGearmentSettings(int ModuleId);
        Models.GearmentSetting GetGearmentSetting(int GearmentSettingId);
        Models.GearmentSetting AddGearmentSetting(Models.GearmentSetting GearmentSetting);
        Models.GearmentSetting UpdateGearmentSetting(Models.GearmentSetting GearmentSetting);
        void DeleteGearmentSetting(int GearmentSettingId);
    }
}
