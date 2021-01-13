using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Gearment.Employee.Models;

namespace Gearment.Employee.Repository
{
    public class EmployeeRepository : IEmployeeRepository, IService
    {
        private readonly EmployeeContext _db;

        public EmployeeRepository(EmployeeContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.Employee> GetEmployees(int ModuleId)
        {
            return _db.Employee.Where(item => item.ModuleId == ModuleId);
        }

        public List<Models.Employee> GetEmployeeByNameOrPayrollId(Models.Employee Employee)
        {
            List<Models.Employee> result = new List<Models.Employee>();
            result = _db.Employee.Where(x => x.PayrollID == Employee.PayrollID || x.Name == Employee.Name).ToList();

            return result;
        }

        public Models.Employee GetEmployee(int EmployeeId)
        {
            return _db.Employee.Find(EmployeeId);
        }

        public Models.Employee AddEmployee(Models.Employee Employee)
        {
            _db.Employee.Add(Employee);
            _db.SaveChanges();
            return Employee;
        }

        public Models.Employee UpdateEmployee(Models.Employee Employee)
        {
            _db.Entry(Employee).State = EntityState.Modified;
            _db.SaveChanges();
            return Employee;
        }

        public IEnumerable<Models.Employee> UpdateEmployees(IEnumerable<Models.Employee> Employees)
        {
            _db.UpdateRange(Employees);
            _db.SaveChanges();
            return Employees;
        }

        public void DeleteEmployee(int EmployeeId)
        {
            Models.Employee Employee = _db.Employee.Find(EmployeeId);
            _db.Employee.Remove(Employee);
            _db.SaveChanges();
        }
    }
}
