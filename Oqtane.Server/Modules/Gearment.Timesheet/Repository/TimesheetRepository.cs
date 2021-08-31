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
        public Models.Employee_FaceRegEvent AddEvent(Models.Employee_FaceRegEvent Employee_FaceRegEvent)
        {
            if (!_db.Employee_FaceRegEvent.Any(x => x.EventTime == Employee_FaceRegEvent.EventTime && x.EventType == Employee_FaceRegEvent.EventType && x.EmployeeId == Employee_FaceRegEvent.EmployeeId))
            {
                _db.Employee_FaceRegEvent.Add(Employee_FaceRegEvent);

            }
            else
            {
                _db.Employee_FaceRegEvent.Update(Employee_FaceRegEvent);
            }

            _db.SaveChanges();
            return Employee_FaceRegEvent;
        }

        public Models.Employee_FaceReg AddFaces(Models.Employee_FaceReg Employee_FaceReg)
        {
            if (!_db.Employee_FaceReg.Any(x => x.EmployeeId == Employee_FaceReg.EmployeeId))
            {
                _db.Employee_FaceReg.Add(Employee_FaceReg);               
            }
            else
            {
                _db.Employee_FaceReg.Add(Employee_FaceReg);
            }

            _db.SaveChanges();

            return Employee_FaceReg;
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

        public Models.Employee_FaceRegEventDetail UpdateEvent(Models.Employee_FaceRegEventDetail Timesheet)
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

        public void DeleteFaceRegEvent(int EventId)
        {
            Models.Employee_FaceRegEvent eventFound = _db.Employee_FaceRegEvent.Find(EventId);
            _db.Employee_FaceRegEvent.Remove(eventFound);
            _db.SaveChanges();
        }

        public Employee_FaceRegEventDetail GetEmployee_FaceRegEvent(int EventId)
        {
            Employee_FaceRegEventDetail employeeDetail = new Employee_FaceRegEventDetail();

            var foundRecord = _db.Employee_FaceRegEvent.Find(EventId);
            if (foundRecord != null)
            {
                var employee = _db.Employee.FirstOrDefault(x => x.EmployeeId == foundRecord.EmployeeId);

                if (employee != null)
                {
                    employeeDetail.EventId = foundRecord.EventId;
                    employeeDetail.EventTime = foundRecord.EventTime;
                    employeeDetail.EventType = foundRecord.EventType;
                    employeeDetail.FaceScore = Math.Round(foundRecord.FaceScore, 2);
                    employeeDetail.Station = foundRecord.Station;

                    employeeDetail.EmployeeId = foundRecord.EmployeeId;
                    employeeDetail.Name = employee.Name;
                    employeeDetail.PayrollID = employee.PayrollID;
                    employeeDetail.Rate = employee.Rate;
                    employeeDetail.Department = employee.Department;
                    employeeDetail.Status = employee.Status;
                    employeeDetail.Note = employee.Note;
                    employeeDetail.StartDate = employee.StartDate;
                    employeeDetail.IsWarning = foundRecord.IsWarning;
                    employeeDetail.ImageUrl = foundRecord.EventTime.Year + "-" + foundRecord.EventTime.ToString("MM") + "-" + foundRecord.EventTime.ToString("dd") + "/" + foundRecord.EventId + ".jpg";
                }
            }

            return employeeDetail;
        }

        public List<Employee_FaceRegEventDetail> GetAllEmployee_FaceRegEvent(TimesheetDailyQuery Query)
        {
            List<Employee_FaceRegEventDetail> result = new List<Employee_FaceRegEventDetail>();

            List<Employee_FaceRegEvent> faceRegEvent = new List<Employee_FaceRegEvent>();
            if (Query.EventId != 0)
            {
                faceRegEvent = _db.Employee_FaceRegEvent.Where(x => x.EventId == Query.EventId).ToList();
            }
            else
            {
                faceRegEvent = _db.Employee_FaceRegEvent.Where(x => x.EventTime.Date >= Query.FromDate.Date && x.EventTime.Date <= Query.ToDate.Date).ToList();
            }

            if (Query.IsWarning)
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
                    employeeDetail.FaceScore = Math.Round(item.FaceScore, 2);
                    employeeDetail.Station = item.Station;

                    employeeDetail.EmployeeId = item.EmployeeId;
                    employeeDetail.Name = employee.Name;
                    employeeDetail.PayrollID = employee.PayrollID;
                    employeeDetail.Rate = employee.Rate;
                    employeeDetail.Department = employee.Department;
                    employeeDetail.Status = employee.Status;
                    employeeDetail.Note = employee.Note;
                    employeeDetail.StartDate = employee.StartDate;
                    employeeDetail.IsWarning = item.IsWarning;
                    employeeDetail.ImageUrl = item.EventTime.Year + "-" + item.EventTime.ToString("MM") + "-" + item.EventTime.ToString("dd") + "/" + item.EventId + ".jpg";

                    result.Add(employeeDetail);
                }
            }

            return result;
        }

        public Models.Employee_FaceReg GetEmployee_FaceReg(int employeeId)
        {
            return _db.Employee_FaceReg.Find(employeeId);
        }
    }
}
