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
            if (!_db.Timesheet.Any(x =>
            string.Equals(x.FirstName, Timesheet.FirstName) &&
            string.Equals(x.LastName, Timesheet.LastName) &&
            string.Equals(x.PayRollID, Timesheet.PayRollID) &&
            string.Equals(x.DayOfWeek, Timesheet.DayOfWeek) &&
            string.Equals(x.Date, Timesheet.Date) &&
            string.Equals(x.In, Timesheet.In) &&
            string.Equals(x.Out, Timesheet.Out) &&
            string.Equals(x.Hours, Timesheet.Hours) &&
            string.Equals(x.Type, Timesheet.Type)))
            {
                _db.Timesheet.Add(Timesheet);
                _db.SaveChanges();

                return Timesheet;
            }

            return null;
        }

        public TimesheetData AddTimesheetData(TimesheetData timesheetData)
        {
            _db.TimesheetData.Add(timesheetData);
            _db.SaveChanges();

            return timesheetData;
        }

        public List<TimesheetData> GetAllTimesheetData()
        {
            return _db.TimesheetData.ToList();
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

        //public void AddTimesheetFilter(GearmentTimesheetFilter filter)
        //{
        //    _db.TimesheetFilter.Add(filter);
        //    _db.SaveChanges();
        //}
    }
}
