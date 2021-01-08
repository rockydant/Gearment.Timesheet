using System;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace Gearment.Department.Models
{
    [Table("GearmentDepartment")]
    public class Department : IAuditable
    {
        public int DepartmentId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }

        public DateTime DailyStartTime { get; set; }
        public DateTime DailyEndTime { get; set; }
        public DateTime BreakStartTime { get; set; }
        public DateTime BreakEndTime { get; set; }
        public int TotalRestHour { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
