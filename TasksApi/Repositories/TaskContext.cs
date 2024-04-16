using Microsoft.EntityFrameworkCore;
using TasksApi.Models;

namespace TasksApi.Repositories
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options) { }

        public DbSet<EmployeeTask> EmployeeTasks { get; set; }

    }
}
