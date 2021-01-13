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
        IEnumerable<Models.Employee> UpdateEmployees(IEnumerable<Models.Employee> Employees);
        void DeleteEmployee(int EmployeeId);

        List<Models.Employee> GetEmployeeByNameOrPayrollId(Models.Employee Employee);
    }
}
