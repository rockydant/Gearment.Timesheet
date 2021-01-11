using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Repository
{
    public class TimesheetRepository : ITimesheetRepository, IService
    {
        private readonly TimesheetContext _db;

        public TimesheetRepository(TimesheetContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.Timesheet> GetTimesheets(int ModuleId)
        {
            return _db.Timesheet.Where(item => item.ModuleId == ModuleId);
        }

        public Models.Timesheet GetTimesheet(int TimesheetId)
        {
            return _db.Timesheet.Find(TimesheetId);
        }

        public Models.Timesheet AddTimesheet(Models.Timesheet Timesheet)
        {
            _db.Timesheet.Add(Timesheet);
            _db.SaveChanges();
            return Timesheet;
        }

        public Models.Timesheet UpdateTimesheet(Models.Timesheet Timesheet)
        {
            _db.Entry(Timesheet).State = EntityState.Modified;
            _db.SaveChanges();
            return Timesheet;
        }

        public void DeleteTimesheet(int TimesheetId)
        {
            Models.Timesheet Timesheet = _db.Timesheet.Find(TimesheetId);
            _db.Timesheet.Remove(Timesheet);
            _db.SaveChanges();
        }
    }
}
