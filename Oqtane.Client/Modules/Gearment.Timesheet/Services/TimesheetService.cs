using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Gearment.Timesheet.Models;
using System;

namespace Gearment.Timesheet.Services
{
    public class TimesheetService : ServiceBase, ITimesheetService, IService
    {
        private readonly SiteState _siteState;

        public TimesheetService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

        private string Apiurl => CreateApiUrl(_siteState.Alias, "Timesheet");

        public async Task<List<Models.Timesheet>> GetTimesheetsAsync(int ModuleId)
        {
            List<Models.Timesheet> Timesheets = await GetJsonAsync<List<Models.Timesheet>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", ModuleId));
            return Timesheets.OrderBy(item => item.FirstName).ToList();
        }

        public async Task<Models.TimesheetData> GetTimesheetAsync(int TimesheetId, int ModuleId)
        {
            return await GetJsonAsync<Models.TimesheetData>(CreateAuthorizationPolicyUrl($"{Apiurl}/{TimesheetId}", ModuleId));
        }

        public async Task<Models.Timesheet> AddTimesheetAsync(Models.Timesheet Timesheet)
        {
            return await PostJsonAsync<Models.Timesheet>(CreateAuthorizationPolicyUrl($"{Apiurl}", Timesheet.ModuleId), Timesheet);
        }

        public async Task<Models.TimesheetData> UpdateTimesheetAsync(Models.TimesheetData Timesheet, int ModuleId)
        {
            return await PutJsonAsync<Models.TimesheetData>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Timesheet.TimesheetDataId}", ModuleId), Timesheet);
        }

        public async Task DeleteTimesheetAsync(int TimesheetId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{TimesheetId}", ModuleId));
        }

        public async Task DeleteTimesheetByDateAsync(int ModuleId, TimesheetDailyQuery Query)
        {
            await PostJsonAsync<TimesheetDailyQuery, List<Models.AttendanceReport>>(CreateAuthorizationPolicyUrl($"{Apiurl}/correct", ModuleId), Query);
        }

        public async Task<Models.TimesheetViewModel> ProcessFileAsync(int moduleId, int fileId)
        {
            return await GetJsonAsync<Models.TimesheetViewModel>($"{Apiurl}/process/{moduleId}/{fileId}");
        }

        public async Task<List<Models.TimesheetData>> GetTimesheetDataAsync(int ModuleId)
        {
            List<Models.TimesheetData> Timesheets = await GetJsonAsync<List<Models.TimesheetData>>(CreateAuthorizationPolicyUrl($"{Apiurl}/data", ModuleId));
            return Timesheets.OrderBy(item => item.Date).ToList();
        }
        public async Task<List<Models.TimesheetDataExcelExport>> GetAttendanceDataAsync(int ModuleId, TimesheetDailyQuery TimesheetDailyQuery)
        {
            return await PostJsonAsync<TimesheetDailyQuery, List<Models.TimesheetDataExcelExport>>(CreateAuthorizationPolicyUrl($"{Apiurl}/attendance/", ModuleId), TimesheetDailyQuery);                     
        }

        public async Task<List<Models.TimesheetData>> GetTimesheetDataByDateAsync(int ModuleId, TimesheetDailyQuery Query)
        {
            List<Models.TimesheetData> Timesheets = await PostJsonAsync<TimesheetDailyQuery, List<Models.TimesheetData>>(CreateAuthorizationPolicyUrl($"{Apiurl}/data", ModuleId), Query);
            return Timesheets.OrderBy(item => item.Date).ToList();
        }

        public async Task<List<Models.Timesheet>> GetTimesheetByDateAsync(int ModuleId, TimesheetDailyQuery Query)
        {
            List<Models.Timesheet> Timesheets = await PostJsonAsync<TimesheetDailyQuery, List<Models.Timesheet>>(CreateAuthorizationPolicyUrl($"{Apiurl}/raw", ModuleId), Query);
            return Timesheets.OrderBy(item => item.Date).ToList();
        }

        public async Task<List<Models.AttendanceReport>> GetTimesheetAttendanceDataAsync(int ModuleId, TimesheetDailyQuery Query)
        {
            List<Models.AttendanceReport> Timesheets = await PostJsonAsync<TimesheetDailyQuery, List<Models.AttendanceReport>>(CreateAuthorizationPolicyUrl($"{Apiurl}/report/", ModuleId), Query);
            return Timesheets.OrderBy(item => item.Date).ToList();
        }

        public async Task DeleteAttendanceDataAsync(int EventId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/attendance/{EventId}", ModuleId));
        }

        public async Task<Models.Employee_FaceRegEventDetail> GetAttendanceDataAsync(int EventId, int ModuleId)
        {
            return await GetJsonAsync<Models.Employee_FaceRegEventDetail>(CreateAuthorizationPolicyUrl($"{Apiurl}/attendance/{EventId}", ModuleId));
        }

        public async Task<Models.Employee_FaceRegEventDetail> UpdateAttendanceDataAsync(Models.Employee_FaceRegEventDetail eventDetail, int ModuleId)
        {
            return await PutJsonAsync<Models.Employee_FaceRegEventDetail>(CreateAuthorizationPolicyUrl($"{Apiurl}/attendance/{eventDetail.EventId}", ModuleId), eventDetail);
        }        
    }
}

