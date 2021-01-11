using System.Collections.Generic;
using System.Threading.Tasks;
using Gearment.Employee.Models;

namespace Gearment.Employee.Services
{
    public interface IEmployeeService 
    {
        Task<List<Department.Models.DepartmentViewModel>> GetDepartmentsAsync(int ModuleId);
        Task<List<Models.Employee>> GetEmployeesAsync(int ModuleId);

        Task<Models.Employee> GetEmployeeAsync(int EmployeeId, int ModuleId);

        Task<Models.Employee> AddEmployeeAsync(Models.Employee Employee);

        Task<Models.Employee> UpdateEmployeeAsync(Models.Employee Employee);

        Task DeleteEmployeeAsync(int EmployeeId, int ModuleId);
    }
}
