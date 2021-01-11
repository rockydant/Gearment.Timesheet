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

        public TimesheetContext(ITenantResolver tenantResolver, IHttpContextAccessor accessor) : base(tenantResolver, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
