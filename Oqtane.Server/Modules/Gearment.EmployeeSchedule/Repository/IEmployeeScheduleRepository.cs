using System.Collections.Generic;
using Gearment.EmployeeSchedule.Models;

namespace Gearment.EmployeeSchedule.Repository
{
    public interface IEmployeeScheduleRepository
    {
        IEnumerable<Models.EmployeeSchedule> GetEmployeeSchedules(int ModuleId);
        Models.EmployeeSchedule GetEmployeeSchedule(int EmployeeScheduleId);
        Models.EmployeeSchedule AddEmployeeSchedule(Models.EmployeeSchedule EmployeeSchedule);
        Models.EmployeeSchedule UpdateEmployeeSchedule(Models.EmployeeSchedule EmployeeSchedule);
        void DeleteEmployeeSchedule(int EmployeeScheduleId);
    }
}
