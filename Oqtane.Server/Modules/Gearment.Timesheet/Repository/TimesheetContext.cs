using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Gearment.Timesheet.Models;

namespace Gearment.Timesheet.Repository
{
    public class TimesheetContext : DBContextBase, IService
    {
        public virtual DbSet<Models.Timesheet> Timesheet { get; set; }
        public virtual DbSet<Models.TimesheetData> TimesheetData { get; set; }
        public virtual DbSet<Models.GearmentEmployee_FaceReg> Employee_FaceReg { get; set; }
        public virtual DbSet<Models.Employee_FaceRegEvent> Employee_FaceRegEvent { get; set; }

        public virtual DbSet<Gearment.Employee.Models.Employee> Employee { get; set; }

        public TimesheetContext(ITenantResolver tenantResolver, IHttpContextAccessor accessor) : base(tenantResolver, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
