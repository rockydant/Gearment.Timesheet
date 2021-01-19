using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Gearment.EmployeeSchedule.Models;

namespace Gearment.EmployeeSchedule.Repository
{
    public class EmployeeScheduleContext : DBContextBase, IService
    {
        public virtual DbSet<Models.EmployeeSchedule> EmployeeSchedule { get; set; }

        public EmployeeScheduleContext(ITenantResolver tenantResolver, IHttpContextAccessor accessor) : base(tenantResolver, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
