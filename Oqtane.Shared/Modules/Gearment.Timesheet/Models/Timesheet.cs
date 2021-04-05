using System;
using System.Collections;
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
        public decimal Rate { get; set; }
        public DateTime DailyStartTime { get; set; }
        public DateTime DailyEndTime { get; set; }
        public DateTime BreakStartTime { get; set; }
        public DateTime BreakEndTime { get; set; }
        public decimal TotalRestHour { get; set; }
        public decimal TotalWorkingHour { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public decimal TotalBreakHour { get; set; }
        public string Notes { get; set; }
    }

    public class TimesheetDataExcelExport
    {
        public int TimesheetDataId { get; set; }
        public string Name { get; set; }        
        public string PayrollID { get; set; }
        public string DayOfWeek { get; set; }
        public string Date { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalPay { get; set; }
        public string DailyStartTime { get; set; }
        public string DailyEndTime { get; set; }
        public string BreakStartTime { get; set; }
        public string BreakEndTime { get; set; }
        public decimal TotalRestHour { get; set; }
        public decimal TotalWorkingHour { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public decimal TotalBreakHour { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
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

    public class TimesheetDailyQuery
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Department { get; set; }
        public string EmployeeName { get; set; }
        public string AttendanceStatus { get; set; }
    }

    public class PayrollViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public List<string[]> WorkingDates { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public decimal TotalPay { get; set; }
    }

    public class AttendanceReport
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Present { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Status { get; set; }
    }

    public class BreakTimes
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
