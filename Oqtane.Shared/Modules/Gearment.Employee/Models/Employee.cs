using System;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace Gearment.Employee.Models
{
    [Table("GearmentEmployee")]
    public class Employee : IAuditable
    {
        public int EmployeeId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }

        public int PayrollID { get; set; }
        public decimal Rate { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
