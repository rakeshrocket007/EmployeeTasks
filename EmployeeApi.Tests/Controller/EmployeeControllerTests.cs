using Castle.Core.Logging;
using EmployeeApi.Repositories;
using EmpoyeeApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using TestApp.Dtos;

namespace EmployeeApi.Tests.Controller
{
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeRepository> employeeRepository;

        private EmployeeController employeeController;

        private List<EmployeeDto> employees;

        [SetUp]
        public void Setup()
        {
            employees = new List<EmployeeDto>();
            employees.Add(new EmployeeDto() { Id = 1, FirstName = "Test First", LastName = "Test Last", Tasks = new List<TaskDto>() });
            employees.Add(new EmployeeDto() { Id = 2, FirstName = "Test First 2", LastName = "Test Last 2", Tasks = new List<TaskDto>() });

            employeeRepository = new Mock<IEmployeeRepository>();
            employeeController = new EmployeeController(employeeRepository.Object, new NullLogger<EmployeeController>());

        }

        [Test]
        public void GetEmployees_Ok()
        {
            employeeRepository.Setup(x => x.GetAllEmployees()).ReturnsAsync(employees.AsEnumerable<EmployeeDto>());

            var result = employeeController.GetAllEmployees() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.Equals(200, result.StatusCode);

            Assert.Equals(2, (result.Value as IEnumerable<EmployeeDto>).Count());
        }

        [Test]
        public void GetEmployees_ThrowsException()
        {
            employeeRepository.Setup(x => x.GetAllEmployees()).Throws(new Exception("Test Exception"));

            var result = employeeController.GetAllEmployees() as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);

        }

        [Test]
        public void GetEmployeeById_Ok()
        {
            employeeRepository.Setup(x => x.GetEmployee(1)).ReturnsAsync(employees[0]);

            var result = employeeController.GetEmployee(1) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            Assert.AreEqual(1, (result.Value as EmployeeDto).Id);
        }

        [Test]
        public void GetEmployeeById_ThrowsException()
        {
            var result = employeeController.GetEmployee(0) as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void InsertEmployee_Ok()
        {
            employeeRepository.Setup(x => x.InsertEmployee(employees[0])).ReturnsAsync(employees[0]);

            var result = employeeController.InsertEmployeeTask(employees[0]) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            Assert.AreEqual(1, (result.Value as EmployeeDto).Id);
        }
    }
}