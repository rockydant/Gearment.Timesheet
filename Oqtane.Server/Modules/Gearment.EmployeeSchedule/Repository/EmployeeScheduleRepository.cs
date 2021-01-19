using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Gearment.EmployeeSchedule.Models;

namespace Gearment.EmployeeSchedule.Repository
{
    public class EmployeeScheduleRepository : IEmployeeScheduleRepository, IService
    {
        private readonly EmployeeScheduleContext _db;

        public EmployeeScheduleRepository(EmployeeScheduleContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.EmployeeSchedule> GetEmployeeSchedules(int ModuleId)
        {
            return _db.EmployeeSchedule.Where(item => item.ModuleId == ModuleId);
        }

        public Models.EmployeeSchedule GetEmployeeSchedule(int EmployeeScheduleId)
        {
            return _db.EmployeeSchedule.Find(EmployeeScheduleId);
        }

        public Models.EmployeeSchedule AddEmployeeSchedule(Models.EmployeeSchedule EmployeeSchedule)
        {
            _db.EmployeeSchedule.Add(EmployeeSchedule);
            _db.SaveChanges();
            return EmployeeSchedule;
        }

        public Models.EmployeeSchedule UpdateEmployeeSchedule(Models.EmployeeSchedule EmployeeSchedule)
        {
            _db.Entry(EmployeeSchedule).State = EntityState.Modified;
            _db.SaveChanges();
            return EmployeeSchedule;
        }

        public void DeleteEmployeeSchedule(int EmployeeScheduleId)
        {
            Models.EmployeeSchedule EmployeeSchedule = _db.EmployeeSchedule.Find(EmployeeScheduleId);
            _db.EmployeeSchedule.Remove(EmployeeSchedule);
            _db.SaveChanges();
        }
    }
}
