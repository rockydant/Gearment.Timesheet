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
        Models.Employee_FaceReg AddFaces(Models.Employee_FaceReg Employee_FaceReg);
        Models.TimesheetData UpdateTimesheet(Models.TimesheetData Timesheet);
        void DeleteTimesheet(int TimesheetId);
        void DeleteTimesheetByDateAsync(TimesheetDailyQuery Query);

        Models.TimesheetData GetTimesheetData(Models.TimesheetData Timesheet);
        List<TimesheetData> GetAllTimesheetData();
        Models.TimesheetData AddTimesheetData(Models.TimesheetData Timesheet);
        Models.TimesheetData UpdateTimesheetData(Models.TimesheetData Timesheet);

        Models.Employee_FaceRegEventDetail GetEmployee_FaceRegEvent(int EventId);
        Models.Employee_FaceReg GetEmployee_FaceReg(int employeeId);
        List<Employee_FaceRegEventDetail> GetAllEmployee_FaceRegEvent(TimesheetDailyQuery Query);

        Models.Employee_FaceRegEventDetail UpdateEvent(Models.Employee_FaceRegEventDetail Timesheet);

        Models.Employee_FaceRegEvent AddEvent(Models.Employee_FaceRegEvent Employee_FaceRegEvent);        

        void DeleteFaceRegEvent(int EventId);
    }
}
