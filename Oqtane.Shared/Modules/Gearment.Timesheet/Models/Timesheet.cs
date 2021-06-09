using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    [Table("GearmentEmployee_FaceRegEvent")]
    public class Employee_FaceRegEvent
    {
        [Key]
        public int EventId { get; set; }
        public DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public int EmployeeId { get; set; }
        public double FaceScore { get; set; }
        public int Station { get; set; }
        public Byte[] PhotoJpg { get; set; }

        public bool IsWarning { get; set; }
    }

    public class Employee_FaceRegEventDetail : Employee_FaceRegEvent
    {
        public string Name { get; set; }
        public int PayrollID { get; set; }
        public decimal Rate { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Employee_FaceRegEventSummary
    {
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public int PayrollID { get; set; }
        public decimal Rate { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public string ImageUrl { get; set; }

        public DateTime EventDate { get; set; }
        public List<Employee_FaceRegEventDetail> EventTimeLine { get; set; }
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
        public int EmployeeId { get; set; }
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

        public List<Employee_FaceRegEventDetail> EventTimeLine { get; set; }
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
        public List<decimal> Hours { get; set; }
        public List<decimal> BreakHours { get; set; }
    }

    public class TimesheetViewModel
    {
        public List<Models.Timesheet> TimesheetList { get; set; }
        public List<Employee.Models.Employee> MissingEmployee { get; set; }
    }

    public class TimesheetDailyQuery
    {
        public int EventId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Department { get; set; }
        public string EmployeeName { get; set; }
        public string AttendanceStatus { get; set; }

        public bool IsWarning { get; set; }
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

    [Table("GearmentEmployee_FaceReg")]
    public class GearmentEmployee_FaceReg
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FaceRegId { get; set; }
        public Byte[] FacePhoto1 { get; set; }
        public Byte[] FacePhoto2 { get; set; }
        public Byte[] FacePhoto3 { get; set; }
    }
}
