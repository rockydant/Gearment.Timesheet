using System;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace Gearment.GearmentSetting.Models
{
    [Table("GearmentGearmentSetting")]
    public class GearmentSetting : IAuditable
    {
        public int GearmentSettingId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
