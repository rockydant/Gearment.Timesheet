using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Gearment.Department.Models;

namespace Gearment.Department.Repository
{
    public class DepartmentContext : DBContextBase, IService
    {
        public virtual DbSet<Models.Department> Department { get; set; }

        public DepartmentContext(ITenantResolver tenantResolver, IHttpContextAccessor accessor) : base(tenantResolver, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
