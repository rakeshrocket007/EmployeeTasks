using EmployeeTasks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestApp.Dtos;

namespace EmployeeTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger) 
        { 
            _employeeService = employeeService;
            _logger = logger;
        }


        [HttpGet]
        [Route("getallemployees")]
        public IActionResult GetAllEmployees()
        {
            _logger.LogInformation("Get All Employees");
            try
            {
                var result = _employeeService.GetAllEmployees().Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }


        }

        [HttpGet]
        [Route("getemployee/{employeeId}")]
        public IActionResult GetEmployee(int employeeId)
        {
            _logger.LogInformation($"Get Employee {employeeId}");
            if (employeeId <= 0) 
            { 
                return BadRequest("The employeeid is not valid."); 
            }

            var result = _employeeService.GetEmployeeById(employeeId).Result;

            if(result == null)
            {
                _logger.LogInformation($"Employee not found with employeeid: {employeeId}");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("insert")]
        public IActionResult InsertEmployee(EmployeeDto employee)
        {
            _logger.LogInformation($"Insert employee : {JsonConvert.SerializeObject(employee)} ");
            if (employee == null)
            {
                _logger.LogInformation("Employee data is null");
                return BadRequest();
            }

            try
            {
                _employeeService.InsertEmployee(employee);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
    }
}
