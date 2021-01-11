using System.Collections.Generic;
using System.Threading.Tasks;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Services
{
    public interface ITimesheetService 
    {
        Task<List<Models.Timesheet>> GetTimesheetsAsync(int ModuleId);

        Task<Models.Timesheet> GetTimesheetAsync(int TimesheetId, int ModuleId);

        Task<Models.Timesheet> AddTimesheetAsync(Models.Timesheet Timesheet);

        Task<Models.Timesheet> UpdateTimesheetAsync(Models.Timesheet Timesheet);

        Task DeleteTimesheetAsync(int TimesheetId, int ModuleId);
    }
}
