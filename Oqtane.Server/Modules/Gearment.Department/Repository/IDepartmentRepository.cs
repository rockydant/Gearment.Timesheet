using System.Collections.Generic;
using Gearment.Department.Models;

namespace Gearment.Department.Repository
{
    public interface IDepartmentRepository
    {
        IEnumerable<Models.DepartmentViewModel> GetDepartments();
        IEnumerable<Models.Department> GetDepartments(int ModuleId);
        Models.Department GetDepartment(int DepartmentId);

        Models.Department GetDepartmentByName(string Name);
        Models.Department AddDepartment(Models.Department Department);
        Models.Department UpdateDepartment(Models.Department Department);
        void DeleteDepartment(int DepartmentId);
    }
}
