using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Gearment.Summary.Models;

namespace Gearment.Summary.Repository
{
    public class SummaryContext : DBContextBase, IService
    {
        public virtual DbSet<Models.Summary> Summary { get; set; }

        public SummaryContext(ITenantResolver tenantResolver, IHttpContextAccessor accessor) : base(tenantResolver, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
