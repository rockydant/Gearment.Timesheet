using System.Collections.Generic;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Repository
{
    public interface ITimesheetRepository
    {
        IEnumerable<Models.Timesheet> GetTimesheets(int ModuleId);
        IEnumerable<Models.TimesheetData> GetTimesheets(string employeeName);
        List<Models.Timesheet> GetAllTimesheet();
        Models.TimesheetData UpdateRateAnDepartment(Gearment.Employee.Models.Employee timesheet);
        Models.TimesheetData GetTimesheet(int TimesheetId);
        Models.Timesheet AddTimesheet(Models.Timesheet Timesheet);
        Models.TimesheetData UpdateTimesheet(Models.TimesheetData Timesheet);
        void DeleteTimesheet(int TimesheetId);
        void DeleteTimesheetByDateAsync(TimesheetDailyQuery Query);

        Models.TimesheetData GetTimesheetData(Models.TimesheetData Timesheet);
        List<TimesheetData> GetAllTimesheetData();
        Models.TimesheetData AddTimesheetData(Models.TimesheetData Timesheet);
        Models.TimesheetData UpdateTimesheetData(Models.TimesheetData Timesheet);

        Models.Employee_FaceRegEventDetail GetEmployee_FaceRegEvent(int EventId);
        List<Employee_FaceRegEventDetail> GetAllEmployee_FaceRegEvent(TimesheetDailyQuery Query);

        void DeleteFaceRegEvent(int EventId);
    }
}
