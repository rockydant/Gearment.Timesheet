using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Gearment.GearmentSetting.Models;

namespace Gearment.GearmentSetting.Repository
{
    public class GearmentSettingContext : DBContextBase, IService
    {
        public virtual DbSet<Models.GearmentSetting> GearmentSetting { get; set; }

        public GearmentSettingContext(ITenantResolver tenantResolver, IHttpContextAccessor accessor) : base(tenantResolver, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
