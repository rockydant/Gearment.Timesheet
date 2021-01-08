using System.Collections.Generic;
using System.Threading.Tasks;
using Gearment.Department.Models;

namespace Gearment.Department.Services
{
    public interface IDepartmentService 
    {
        Task<List<Models.Department>> GetDepartmentsAsync(int ModuleId);

        Task<Models.Department> GetDepartmentAsync(int DepartmentId, int ModuleId);

        Task<Models.Department> AddDepartmentAsync(Models.Department Department);

        Task<Models.Department> UpdateDepartmentAsync(Models.Department Department);

        Task DeleteDepartmentAsync(int DepartmentId, int ModuleId);
    }
}
