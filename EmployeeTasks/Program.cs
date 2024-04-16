using EmployeeTasks.Helpers.Middleware;
using EmployeeTasks.Services;
using Serilog;
using TestApp.Dtos;

namespace EmployeeTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog with Correlation id
            builder.Services.AddHttpContextAccessor();
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            // Add services to the container.

            builder.Services.AddControllers();

            // Added Swagger Support.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Configure HttpClient
            builder.Services.AddHttpClient("employeeService", (serviceProvider, httpClient) =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:7263/api/employee/");
            });
            builder.Services.AddHttpClient("taskService", (serviceProvider, httpClient) =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:7215/api/task/");
            });

            //Added as singleton to push start up data else can be scoped or transient.
            builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Sample Employee Data
            var employeeService = app.Services.GetRequiredService<IEmployeeService>();
            employeeService.InsertEmployee(new EmployeeDto() { FirstName = "Rogerer", LastName = "Federer", Tasks = new List<TaskDto> { new TaskDto() { Description = "Autograph Task" } } });
            employeeService.InsertEmployee(new EmployeeDto() { FirstName = "Virat", LastName = "Kohli", Tasks = new List<TaskDto> { new TaskDto() { Description = "Photo Task" } } });


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}