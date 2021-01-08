using System.Collections.Generic;
using Gearment.Employee.Models;

namespace Gearment.Employee.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<Models.Employee> GetEmployees(int ModuleId);
        Models.Employee GetEmployee(int EmployeeId);
        Models.Employee AddEmployee(Models.Employee Employee);
        Models.Employee UpdateEmployee(Models.Employee Employee);
        void DeleteEmployee(int EmployeeId);
    }
}
