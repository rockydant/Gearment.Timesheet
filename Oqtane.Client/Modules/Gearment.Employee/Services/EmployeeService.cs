using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Gearment.Employee.Models;

namespace Gearment.Employee.Services
{
    public class EmployeeService : ServiceBase, IEmployeeService, IService
    {
        private readonly SiteState _siteState;

        public EmployeeService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

         private string Apiurl => CreateApiUrl(_siteState.Alias, "Employee");

        public async Task<List<Models.Employee>> GetEmployeesAsync(int ModuleId)
        {
            List<Models.Employee> Employees = await GetJsonAsync<List<Models.Employee>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", ModuleId));
            return Employees.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Employee> GetEmployeeAsync(int EmployeeId, int ModuleId)
        {
            return await GetJsonAsync<Models.Employee>(CreateAuthorizationPolicyUrl($"{Apiurl}/{EmployeeId}", ModuleId));
        }

        public async Task<Models.Employee> AddEmployeeAsync(Models.Employee Employee)
        {
            return await PostJsonAsync<Models.Employee>(CreateAuthorizationPolicyUrl($"{Apiurl}", Employee.ModuleId), Employee);
        }

        public async Task<Models.Employee> UpdateEmployeeAsync(Models.Employee Employee)
        {
            return await PutJsonAsync<Models.Employee>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Employee.EmployeeId}", Employee.ModuleId), Employee);
        }

        public async Task DeleteEmployeeAsync(int EmployeeId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{EmployeeId}", ModuleId));
        }

        public async Task<List<Department.Models.DepartmentViewModel>> GetDepartmentsAsync(int ModuleId)
        {
            return await GetJsonAsync<List<Department.Models.DepartmentViewModel>>(CreateAuthorizationPolicyUrl($"{Apiurl}/departments", ModuleId));
            
        }
    }
}
