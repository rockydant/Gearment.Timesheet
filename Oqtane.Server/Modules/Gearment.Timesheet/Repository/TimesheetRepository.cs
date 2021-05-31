using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Gearment.Timesheet.Models;
using Gearment.Employee.Models;
using System;

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

        public IEnumerable<Models.TimesheetData> GetTimesheets(string employeeName)
        {
            return _db.TimesheetData.Where(item => (item.FirstName + " " + item.LastName) == employeeName);
        }

        public Models.TimesheetData GetTimesheet(int TimesheetDataId)
        {
            return _db.TimesheetData.Find(TimesheetDataId);
        }

        public Models.TimesheetData UpdateRateAnDepartment(Gearment.Employee.Models.Employee timesheet)
        {
            var data = _db.TimesheetData.Where(item => (item.FirstName + " " + item.LastName) == timesheet.Name).ToList();
            foreach (var item in data)
            {
                item.Rate = timesheet.Rate;
                item.Department = timesheet.Department;
                _db.TimesheetData.Update(item);
            }

            _db.SaveChanges();

            return data.FirstOrDefault();
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

        public TimesheetData GetTimesheetData(TimesheetData timesheetData)
        {
            return _db.TimesheetData.Where(item => item.FirstName == timesheetData.FirstName && item.LastName == timesheetData.LastName && item.Date == timesheetData.Date).FirstOrDefault();
        }

        public Models.TimesheetData UpdateTimesheetData(Models.TimesheetData Timesheet)
        {
            _db.Entry(Timesheet).State = EntityState.Modified;
            _db.SaveChanges();
            return Timesheet;
        }

        public List<TimesheetData> GetAllTimesheetData()
        {
            return _db.TimesheetData.ToList();
        }

        public List<Models.Timesheet> GetAllTimesheet()
        {
            return _db.Timesheet.ToList();
        }

        public List<Models.Timesheet> GetTimesheetByDate(TimesheetDailyQuery Query)
        {
            var result = GetAllTimesheet().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= Query.ToDate).ToList();
            return result;
        }

        public void DeleteTimesheetByDateAsync(TimesheetDailyQuery Query)
        {
            var removeTimesheetList = _db.Timesheet.ToList().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= Query.ToDate);

            foreach (var item in removeTimesheetList)
            {
                _db.Timesheet.Remove(item);
            }

            var removeTimesheetDataList = _db.TimesheetData.ToList().Where(x => DateTime.Parse(x.Date) >= Query.FromDate && DateTime.Parse(x.Date) <= Query.ToDate);
            foreach (var item in removeTimesheetDataList)
            {
                _db.TimesheetData.Remove(item);
            }

            _db.SaveChanges();

        }

        public Models.TimesheetData UpdateTimesheet(Models.TimesheetData Timesheet)
        {
            _db.Entry(Timesheet).State = EntityState.Modified;
            _db.SaveChanges();
            return Timesheet;
        }

        public void DeleteTimesheet(int TimesheetId)
        {
            Models.TimesheetData Timesheet = _db.TimesheetData.Find(TimesheetId);
            _db.TimesheetData.Remove(Timesheet);
            _db.SaveChanges();
        }

        public Employee_FaceRegEvent GetEmployee_FaceRegEvent(Employee_FaceRegEvent Employee_FaceRegEvent)
        {
            throw new NotImplementedException();
        }

        public List<Employee_FaceRegEventDetail> GetAllEmployee_FaceRegEvent(bool IsWarning)
        {
            List<Employee_FaceRegEventDetail> result = new List<Employee_FaceRegEventDetail>();
            var faceRegEvent = _db.Employee_FaceRegEvent.ToList();
            if (IsWarning)
            {
                faceRegEvent = faceRegEvent.Where(x => x.IsWarning).ToList();
            }

            foreach (var item in faceRegEvent)
            {
                var employee = _db.Employee.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                if (employee != null)
                {
                    Employee_FaceRegEventDetail employeeDetail = new Employee_FaceRegEventDetail();
                    employeeDetail.EventId = item.EventId;
                    employeeDetail.EventTime = item.EventTime;
                    employeeDetail.EventType = item.EventType;
                    employeeDetail.FaceScore = item.FaceScore;
                    employeeDetail.Station = item.Station;

                    employeeDetail.EmployeeId = item.EmployeeId;
                    employeeDetail.Name = employee.Name;
                    employeeDetail.PayrollID = employee.PayrollID;
                    employeeDetail.Rate = employee.Rate;
                    employeeDetail.Department = employee.Department;
                    employeeDetail.Status = employee.Status;
                    employeeDetail.Note = employee.Note;
                    employeeDetail.StartDate = employee.StartDate;

                    if (IsWarning)
                    {
                        employeeDetail.ImageUrl = item.EventTime.Year + "-" + item.EventTime.Month + "-" + item.EventTime.Day + "/" + item.EventId + ".jpg";
                    }
                    else
                    {
                        employeeDetail.ImageUrl = string.Empty;
                    }

                    result.Add(employeeDetail);
                }
            }

            return result;
        }
        //public void AddTimesheetFilter(GearmentTimesheetFilter filter)
        //{
        //    _db.TimesheetFilter.Add(filter);
        //    _db.SaveChanges();
        //}
    }
}
