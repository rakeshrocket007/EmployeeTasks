using AutoMapper;
using EmployeeApi.Models;
using TestApp.Dtos;

namespace EmployeeApi.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        protected EmployeeDbContext dbContext;
        private IMapper _Mapper;
        public EmployeeRepository(EmployeeDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _Mapper = mapper;
        }

        public async  Task<IEnumerable<EmployeeDto>> GetAllEmployees()
        {
            var employees = this.dbContext.Employees.ToList();

            return _Mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> GetEmployee(int id)
        {
            var employee = this.dbContext.Employees.FirstOrDefault(e => e.Id == id);

            return _Mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> InsertEmployee(EmployeeDto employee)
        {
            var employeeModel = _Mapper.Map<Employee>(employee);
            this.dbContext.Employees.Add(employeeModel);
            this.dbContext.SaveChanges();

            return _Mapper.Map<EmployeeDto>(employeeModel);
        }
    }


    // Added the repositories in the same class instead of seperate folder.
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> InsertEmployee(EmployeeDto employee);

        Task<EmployeeDto> GetEmployee(int id);

        Task<IEnumerable<EmployeeDto>> GetAllEmployees();
    }
}
