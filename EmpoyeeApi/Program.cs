using AutoMapper;
using EmployeeApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TasksApi.Helpers;

namespace EmpoyeeApi
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

            // AutoMapper Configuration
            builder.Services.AddSingleton(new MapperConfiguration(c =>
            {
                c.AddProfile(new EmployeeMappings());
            }).CreateMapper());

            // Dependency Injection
            builder.Services.AddDbContext<EmployeeDbContext>(options =>
                options.UseInMemoryDatabase("employee"));
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}