using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace Gearment.Timesheet.Models
{
    [Table("GearmentTimesheet")]
    public class Timesheet : IAuditable
    {
        public int TimesheetId { get; set; }
        public int ModuleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayRollID { get; set; }
        public string DayOfWeek { get; set; }
        public string Date { get; set; }
        public string In { get; set; }
        public string Out { get; set; }
        public string Hours { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    //[Table("GearmentTimesheetFilter")]
    public class GearmentTimesheetFilter
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    [Table("GearmentTimesheetData")]    
    public class TimesheetData
    {
        public int TimesheetDataId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string PayrollID { get; set; }
        public string DayOfWeek { get; set; }
        public string Date { get; set; }
        public double Rate { get; set; }
        public DateTime DailyStartTime { get; set; }
        public DateTime DailyEndTime { get; set; }
        public DateTime BreakStartTime { get; set; }
        public DateTime BreakEndTime { get; set; }
        public int TotalRestHour { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
    }

    public class TimesheetDataViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayrollID { get; set; }
        public string DayOfWeek { get; set; }
        public string Date { get; set; }       
        public List<DateTime> InRecords { get; set; }
        public List<DateTime> OutRecords { get; set; }
    }

    public class TimesheetViewModel
    {
        public List<Models.Timesheet> TimesheetList { get; set; }
        public List<Employee.Models.Employee> MissingEmployee { get; set; }
    }
}
