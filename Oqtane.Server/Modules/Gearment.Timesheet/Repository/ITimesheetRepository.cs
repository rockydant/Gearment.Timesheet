using System.Collections.Generic;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Repository
{
    public interface ITimesheetRepository
    {
        IEnumerable<Models.Timesheet> GetTimesheets(int ModuleId);
        Models.TimesheetData GetTimesheet(int TimesheetId);
        Models.Timesheet AddTimesheet(Models.Timesheet Timesheet);
        Models.TimesheetData UpdateTimesheet(Models.TimesheetData Timesheet);
        void DeleteTimesheet(int TimesheetId);

        Models.TimesheetData AddTimesheetData(Models.TimesheetData Timesheet);

        List<TimesheetData> GetAllTimesheetData();

        //void AddTimesheetFilter(GearmentTimesheetFilter filter);
    }
}
