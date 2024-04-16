using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TasksApi.Helpers;
using TasksApi.Repositories;

namespace TasksApi
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

            // Dependency Injection
            builder.Services.AddDbContext<TaskDbContext>(options =>
                options.UseInMemoryDatabase("employee"));
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();

            // AutoMapper Configuration
            builder.Services.AddSingleton(new MapperConfiguration(c =>
            {
                c.AddProfile(new TaskMappings());
            }).CreateMapper());

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}