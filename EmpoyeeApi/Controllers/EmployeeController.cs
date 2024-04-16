using EmployeeApi.Models;
using EmployeeApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TestApp.Dtos;

namespace EmpoyeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository repository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("getallemployees")]
        public IActionResult GetAllEmployees()
        {
            _logger.LogInformation("Get All Employees");

            try
            {
                return Ok(_employeeRepository.GetAllEmployees().Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet]
        [Route("getemployee/{employeeid}")]
        public IActionResult GetEmployee(int employeeid)
        {
            _logger.LogInformation($"Get Employee {employeeid}");

            if (employeeid <= 0)
            {
                return NotFound();
            }

            var result =  _employeeRepository.GetEmployee(employeeid);

            if (result.Result == null)
            {
                _logger.LogInformation($"Employee not found with employeeid: {employeeid}");
                return NotFound();
            }

            return Ok(result.Result);
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult InsertEmployeeTask(EmployeeDto employee)
        {
            _logger.LogInformation($"Insert employee : {JsonConvert.SerializeObject(employee)} ");
            if (employee == null)
            {
                _logger.LogInformation("Employee data is null");
                return BadRequest();
            }

            try
            {
                return Ok(_employeeRepository.InsertEmployee(employee).Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }                        
        }
    }
}