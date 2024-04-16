using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using TestApp.Dtos;

namespace EmployeeTasks.Services
{
    public class EmployeeService : IEmployeeService
    {
        private HttpClient employeeClient;
        private HttpClient taskClient;
        public EmployeeService(IHttpClientFactory clientFactory) 
        {
            employeeClient = clientFactory.CreateClient("employeeService");
            taskClient = clientFactory.CreateClient("taskService");
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployees()
        {
            var result = new List<EmployeeDto>();
            var employeeResult = employeeClient.GetFromJsonAsync<IEnumerable<EmployeeDto>>($"getallemployees");
            var taskResult = taskClient.GetFromJsonAsync<IEnumerable<TaskDto>>($"getalltasks");

            Task.WaitAll(employeeResult, taskResult);
            var employees = await employeeResult;
            var tasks = await taskResult;

            foreach(var employee in employees)
            {
                var employeeTasks = tasks.Where(t => t.EmployeeId == employee.Id).ToList();
                result.Add(new EmployeeDto() { Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName, Tasks = employeeTasks });
            }
            return result; 
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            try
            {
                var employeeResult = employeeClient.GetFromJsonAsync<EmployeeDto>($"getemployee/{id}");
                var taskResult = taskClient.GetFromJsonAsync<IEnumerable<TaskDto>>($"gettasks/{id}");

                Task.WaitAll(employeeResult, taskResult);
                var employee = await employeeResult;
                if (employee == null) return null;

                employee.Tasks = (await taskResult)?.ToList();
                return employee;
            }            
            catch(AggregateException ex)
            {
                if (ex.InnerException is HttpRequestException && (ex.InnerException as HttpRequestException).StatusCode == HttpStatusCode.NotFound)
                    return null;

                throw ex;
            }
        }

        public async Task InsertEmployee(EmployeeDto employee)
        {
            try
            {
                var result = await employeeClient.PostAsJsonAsync<EmployeeDto>($"insert", employee);
                result.EnsureSuccessStatusCode();

                var employeeResult = JsonConvert.DeserializeObject<EmployeeDto>(await result.Content.ReadAsStringAsync());
                employee.Tasks.ForEach(x => {
                    x.EmployeeId = employeeResult.Id;
                    });
                await taskClient.PostAsJsonAsync<IEnumerable<TaskDto>>($"insert", employee.Tasks);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface IEmployeeService
    {
        Task InsertEmployee(EmployeeDto employee);

        Task<IEnumerable<EmployeeDto>> GetAllEmployees();

        Task<EmployeeDto> GetEmployeeById(int id);
    }
}
