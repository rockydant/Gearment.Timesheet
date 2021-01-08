using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Gearment.Employee.Models;

namespace Gearment.Employee.Repository
{
    public class EmployeeContext : DBContextBase, IService
    {
        public virtual DbSet<Models.Employee> Employee { get; set; }

        public EmployeeContext(ITenantResolver tenantResolver, IHttpContextAccessor accessor) : base(tenantResolver, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
