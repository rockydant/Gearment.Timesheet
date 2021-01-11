using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Gearment.Department.Models;

namespace Gearment.Department.Repository
{
    public class DepartmentRepository : IDepartmentRepository, IService
    {
        private readonly DepartmentContext _db;

        public DepartmentRepository(DepartmentContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.DepartmentViewModel> GetDepartments()
        {
            List<DepartmentViewModel> result = new List<DepartmentViewModel>();

            if (_db.Department.Any())
            {
                foreach (var item in _db.Department)
                {
                    result.Add(new DepartmentViewModel
                    {
                        DepartmentId = item.DepartmentId,
                        Name = item.Name
                    });
                }
            }

            return result;                
        }

        public IEnumerable<Models.Department> GetDepartments(int ModuleId)
        {
            return _db.Department.Where(item => item.ModuleId == ModuleId);
        }

        public Models.Department GetDepartment(int DepartmentId)
        {
            return _db.Department.Find(DepartmentId);
        }

        public Models.Department AddDepartment(Models.Department Department)
        {
            _db.Department.Add(Department);
            _db.SaveChanges();
            return Department;
        }

        public Models.Department UpdateDepartment(Models.Department Department)
        {
            _db.Entry(Department).State = EntityState.Modified;
            _db.SaveChanges();
            return Department;
        }

        public void DeleteDepartment(int DepartmentId)
        {
            Models.Department Department = _db.Department.Find(DepartmentId);
            _db.Department.Remove(Department);
            _db.SaveChanges();
        }
    }
}
