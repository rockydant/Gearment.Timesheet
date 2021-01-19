using System;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace Gearment.EmployeeSchedule.Models
{
    [Table("GearmentEmployeeSchedule")]
    public class EmployeeSchedule : IAuditable
    {
        public int EmployeeScheduleId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
