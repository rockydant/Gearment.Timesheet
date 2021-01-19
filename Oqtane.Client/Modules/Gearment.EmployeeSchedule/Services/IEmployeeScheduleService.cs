using System.Collections.Generic;
using System.Threading.Tasks;
using Gearment.EmployeeSchedule.Models;

namespace Gearment.EmployeeSchedule.Services
{
    public interface IEmployeeScheduleService 
    {
        Task<List<Models.EmployeeSchedule>> GetEmployeeSchedulesAsync(int ModuleId);

        Task<Models.EmployeeSchedule> GetEmployeeScheduleAsync(int EmployeeScheduleId, int ModuleId);

        Task<Models.EmployeeSchedule> AddEmployeeScheduleAsync(Models.EmployeeSchedule EmployeeSchedule);

        Task<Models.EmployeeSchedule> UpdateEmployeeScheduleAsync(Models.EmployeeSchedule EmployeeSchedule);

        Task DeleteEmployeeScheduleAsync(int EmployeeScheduleId, int ModuleId);
    }
}
