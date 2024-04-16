using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TasksApi.Models;
using TasksApi.Repositories;
using TestApp.Dtos;

namespace TasksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskRepository _taskRepository;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository repository, ILogger<TaskController> logger)
        {
            _taskRepository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("getalltasks")]
        public IActionResult GetTasks()
        {
            _logger.LogInformation("Get All Tasks");

            try
            {
                return Ok(_taskRepository.GetAllTasks().Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("gettasks/{employeeId}")]
        public IActionResult GetTasks(int employeeId)
        {
            _logger.LogInformation($"Get Tasks for Employee {employeeId}");

            if (employeeId <= 0)
            {
                return BadRequest();
            }

            return Ok(_taskRepository.GetTasks(employeeId).Result);            

        }

        [Route("insert")]
        [HttpPost]
        public IActionResult InsertEmployeeTask(IEnumerable<TaskDto> tasks)
        {
            _logger.LogInformation($"Insert tasks for employee : {JsonConvert.SerializeObject(tasks)} ");
            if (tasks == null)
            {
                _logger.LogInformation("Task data is null");
                return BadRequest();
            }

            try
            {
                return Ok(_taskRepository.InsertTask(tasks).IsCompleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
