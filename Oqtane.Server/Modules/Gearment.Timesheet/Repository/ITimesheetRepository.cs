using System.Collections.Generic;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Repository
{
    public interface ITimesheetRepository
    {
        IEnumerable<Models.Timesheet> GetTimesheets(int ModuleId);
        IEnumerable<Models.TimesheetData> GetTimesheets(string employeeName);
        Models.TimesheetData UpdateRateAnDepartment(Gearment.Employee.Models.Employee timesheet);
        Models.TimesheetData GetTimesheet(int TimesheetId);
        Models.Timesheet AddTimesheet(Models.Timesheet Timesheet);
        Models.TimesheetData UpdateTimesheet(Models.TimesheetData Timesheet);
        void DeleteTimesheet(int TimesheetId);

        void DeleteTimesheetByDateAsync(TimesheetDailyQuery Query);

        Models.TimesheetData AddTimesheetData(Models.TimesheetData Timesheet);

        Models.TimesheetData GetTimesheetData(Models.TimesheetData Timesheet);
        Models.TimesheetData UpdateTimesheetData(Models.TimesheetData Timesheet);

        List<TimesheetData> GetAllTimesheetData();
        List<Models.Timesheet> GetAllTimesheet();

        //void AddTimesheetFilter(GearmentTimesheetFilter filter);
    }
}
