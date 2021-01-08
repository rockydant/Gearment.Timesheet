using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Gearment.Department.Models;

namespace Gearment.Department.Services
{
    public class DepartmentService : ServiceBase, IDepartmentService, IService
    {
        private readonly SiteState _siteState;

        public DepartmentService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

         private string Apiurl => CreateApiUrl(_siteState.Alias, "Department");

        public async Task<List<Models.Department>> GetDepartmentsAsync(int ModuleId)
        {
            List<Models.Department> Departments = await GetJsonAsync<List<Models.Department>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", ModuleId));
            return Departments.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Department> GetDepartmentAsync(int DepartmentId, int ModuleId)
        {
            return await GetJsonAsync<Models.Department>(CreateAuthorizationPolicyUrl($"{Apiurl}/{DepartmentId}", ModuleId));
        }

        public async Task<Models.Department> AddDepartmentAsync(Models.Department Department)
        {
            return await PostJsonAsync<Models.Department>(CreateAuthorizationPolicyUrl($"{Apiurl}", Department.ModuleId), Department);
        }

        public async Task<Models.Department> UpdateDepartmentAsync(Models.Department Department)
        {
            return await PutJsonAsync<Models.Department>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Department.DepartmentId}", Department.ModuleId), Department);
        }

        public async Task DeleteDepartmentAsync(int DepartmentId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{DepartmentId}", ModuleId));
        }
    }
}
