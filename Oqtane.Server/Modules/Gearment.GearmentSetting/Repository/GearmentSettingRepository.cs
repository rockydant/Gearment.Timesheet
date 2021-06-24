using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Gearment.GearmentSetting.Models;

namespace Gearment.GearmentSetting.Repository
{
    public class GearmentSettingRepository : IGearmentSettingRepository, IService
    {
        private readonly GearmentSettingContext _db;

        public GearmentSettingRepository(GearmentSettingContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.GearmentSetting> GetGearmentSettings(int ModuleId)
        {
            return _db.GearmentSetting.ToList();
        }

        public Models.GearmentSetting GetGearmentSetting(int GearmentSettingId)
        {
            return _db.GearmentSetting.Find(GearmentSettingId);
        }

        public Models.GearmentSetting AddGearmentSetting(Models.GearmentSetting GearmentSetting)
        {
            _db.GearmentSetting.Add(GearmentSetting);
            _db.SaveChanges();
            return GearmentSetting;
        }

        public Models.GearmentSetting UpdateGearmentSetting(Models.GearmentSetting GearmentSetting)
        {
            _db.Entry(GearmentSetting).State = EntityState.Modified;
            _db.SaveChanges();
            return GearmentSetting;
        }

        public void DeleteGearmentSetting(int GearmentSettingId)
        {
            Models.GearmentSetting GearmentSetting = _db.GearmentSetting.Find(GearmentSettingId);
            _db.GearmentSetting.Remove(GearmentSetting);
            _db.SaveChanges();
        }
    }
}
