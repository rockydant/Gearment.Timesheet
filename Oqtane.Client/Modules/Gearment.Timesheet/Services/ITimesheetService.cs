using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Services
{
    public interface ITimesheetService
    {
        Task<List<Models.Timesheet>> GetTimesheetsAsync(int ModuleId);

        Task<Models.TimesheetData> GetTimesheetAsync(int TimesheetId, int ModuleId);

        Task<Models.Timesheet> AddTimesheetAsync(Models.Timesheet Timesheet);

        Task<Models.TimesheetData> UpdateTimesheetAsync(Models.TimesheetData Timesheet, int ModuleId);

        Task DeleteTimesheetAsync(int TimesheetId, int ModuleId);

        Task DeleteTimesheetByDateAsync(int ModuleId, TimesheetDailyQuery TimesheetDailyQuery);

        Task<Models.TimesheetViewModel> ProcessFileAsync(int ModuleId, int fileId);

        Task<List<Models.TimesheetData>> GetTimesheetDataAsync(int ModuleId);

        Task<List<Models.TimesheetData>> GetTimesheetDataByDateAsync(int ModuleId, TimesheetDailyQuery TimesheetDailyQuery);
        Task<List<Models.Timesheet>> GetTimesheetByDateAsync(int ModuleId, TimesheetDailyQuery TimesheetDailyQuery);

        Task<List<Models.AttendanceReport>> GetTimesheetAttendanceDataAsync(int ModuleId, TimesheetDailyQuery TimesheetDailyQuery);

        Task<List<Models.TimesheetDataExcelExport>> GetAttendanceDataAsync(int ModuleId, TimesheetDailyQuery TimesheetDailyQuery);

        Task<Models.Employee_FaceRegEventDetail> GetAttendanceDataAsync(int EventId, int ModuleId);
        Task<Models.Employee_FaceRegEventDetail> UpdateAttendanceDataAsync(Models.Employee_FaceRegEventDetail eventDetail, int ModuleId);
        Task DeleteAttendanceDataAsync(int EventId, int ModuleId);
    }
}
