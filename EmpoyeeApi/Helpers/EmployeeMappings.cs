using AutoMapper;
using EmployeeApi.Models;
using TestApp.Dtos;

namespace TasksApi.Helpers
{
    public class EmployeeMappings : Profile
    {
        public EmployeeMappings() { 
            SetMappingConfiguration();
        }

        private void SetMappingConfiguration()
        {
            CreateMap<EmployeeDto, Employee>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName));

            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName));
        }
    }
}
