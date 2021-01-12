using System;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace Gearment.Timesheet.Models
{
    [Table("GearmentTimesheet")]
    public class Timesheet : IAuditable
    {
        public int TimesheetId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class TimesheetViewModel
    {
        public string Employee { get; set; }
        public string PayRollID { get; set; }
        public string DayOfWeek { get; set; }
        public string Date { get; set; }
        public string In { get; set; }
        public string Out { get; set; }
        public string Hours { get; set; }
        public string Type { get; set; }

    }
}
