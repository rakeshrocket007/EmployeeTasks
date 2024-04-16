using Newtonsoft.Json;
using TestApp.Dtos;

namespace EmployeeBlazorServerApp.Services
{
    public class EmployeeService
    {
        private IHttpClientFactory _httpFactory;

        public EmployeeService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployees()
        {
            var employeeClient = _httpFactory.CreateClient("employeetasksservice");

            var employeeResult = await employeeClient.GetFromJsonAsync<IEnumerable<EmployeeDto>>($"getallemployees");

            return employeeResult;
        }

        public async Task<bool> AddEmployee(EmployeeDto employee)
        {
            try
            {
                var employeeClient = _httpFactory.CreateClient("employeetasksservice");

                var employeeResult = await employeeClient.PostAsJsonAsync<EmployeeDto>($"insert", employee);

                employeeResult.EnsureSuccessStatusCode();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
