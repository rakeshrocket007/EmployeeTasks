using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksApi.Models;
using TestApp.Dtos;

namespace TasksApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        internal TaskDbContext _Context;
        internal IMapper _Mapper;

        public TaskRepository(TaskDbContext context, IMapper mapper) {
            _Context = context;
            _Mapper = mapper;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasks()
        {
            var tasks = _Context.EmployeeTasks.ToList();

            return _Mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetTaskById(int taskId)
        {
            var task = _Context.EmployeeTasks.FirstOrDefault(t => t.TaskId == taskId);

            return _Mapper.Map<TaskDto>(task);
        }

        public async Task<IEnumerable<TaskDto>> GetTasks(int employeeId)
        {
            var tasks = _Context.EmployeeTasks.Where(x => x.EmployeeId == employeeId);

            return _Mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task InsertTask(IEnumerable<TaskDto> tasks)
        {
            Thread.Sleep(2000);
            var employeeTasks = _Mapper.Map<IEnumerable<EmployeeTask>>(tasks);

            _Context.EmployeeTasks.AddRange(employeeTasks);
            await _Context.SaveChangesAsync();
        }
    }

    public interface ITaskRepository
    {
        Task InsertTask(IEnumerable<TaskDto> tasks);

        Task<TaskDto> GetTaskById(int id);

        Task<IEnumerable<TaskDto>> GetTasks(int employeeId);

        Task<IEnumerable<TaskDto>> GetAllTasks();
    }
}
