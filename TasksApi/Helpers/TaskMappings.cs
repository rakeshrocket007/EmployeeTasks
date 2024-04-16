using AutoMapper;
using TasksApi.Models;
using TestApp.Dtos;


namespace TasksApi.Helpers
{
    public class TaskMappings : Profile
    {
        public TaskMappings() { 
            SetMappingConfiguration();
        }

        private void SetMappingConfiguration()
        {
            CreateMap<TaskDto, EmployeeTask>()
                .ForMember(d => d.TaskId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.EmployeeId))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));

            CreateMap<EmployeeTask, TaskDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.TaskId))
                .ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.EmployeeId))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
        }
    }
}
