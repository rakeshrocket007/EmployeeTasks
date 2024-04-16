using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TasksApi.Controllers;
using TasksApi.Repositories;
using TestApp.Dtos;

namespace TasksApi.Tests.Controllers
{
    public class TaskApiTests
    {
        private Mock<ITaskRepository> taskRepository;

        private TaskController tasksController;

        private List<TaskDto> tasks;

        [SetUp]
        public void Setup()
        {
            tasks = new List<TaskDto>();
            tasks.Add(new TaskDto() { Id = 1, Description = "Test Task 1", EmployeeId = 1 });
            tasks.Add(new TaskDto() { Id = 2, Description = "Test Task 1", EmployeeId = 1 });
            tasks.Add(new TaskDto() { Id = 3, Description = "Test Task 1", EmployeeId = 2 });

            taskRepository = new Mock<ITaskRepository>();
            tasksController = new TaskController(taskRepository.Object, new NullLogger<TaskController>());

        }

        [Test]
        public void GetTasks_Ok()
        {
            taskRepository.Setup(x => x.GetAllTasks()).ReturnsAsync(tasks.AsEnumerable<TaskDto>());

            var result = tasksController.GetAllTasks() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            Assert.AreEqual(3, (result.Value as IEnumerable<TaskDto>).Count());
        }

        [Test]
        public void GetTasks_ThrowsException()
        {
            taskRepository.Setup(x => x.GetAllTasks()).Throws(new Exception("Test Exception"));

            var result = tasksController.GetAllTasks() as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);

        }

        [Test]
        public void GetTasksById_Ok()
        {
            taskRepository.Setup(x => x.GetTasks(1)).ReturnsAsync(tasks.Where(t => t.EmployeeId == 1));

            var result = tasksController.GetTasks(1) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            Assert.AreEqual(2, (result.Value as IEnumerable<TaskDto>).Count());
        }

        [Test]
        public void GetTasksById_ThrowsException()
        {
            var result = tasksController.GetTasks(0) as BadRequestResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void InsertTask_Ok()
        {
            taskRepository.Setup(x => x.InsertTask(It.IsAny<IEnumerable<TaskDto>>())).Verifiable();

            var result = tasksController.InsertEmployeeTask(tasks) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

        }
    }
}