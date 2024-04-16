There are 5 application projects and 2 unit test project in total in the solution.

1. EmployeeTasks - It is the main api which can call EmployeeApi and TasksApi for their respective operations.
2. EmployeeApi, TasksApi - Contains CRUD operations for their respective entities
3. EmployeeBlazorServerApp - Blazor frontend for performing Add, Read Employees. The functionality in it :
	- Employee List : Show list of Employees (Id, First name, Last name, number of tasks)
	- Add Employee : To Add an employee. Currently adding a random numebr of tasks for each employee.
4. EmployeeApi.Tests, TasksApi.Tests - Added a unit test project for EmployeeApi and TasksApi.

Following features are added into the project:
1. Logging using Serilog
2. Correlation Id for microservices logging
3. Exception handling
4. Used IMapper for creating adapters between Dtos and Entities
5. Added a Blazor app to perform the Create and Read operations
6. DI using .net core
7. Using IHttpClientFactory to enable connection pooling etc instead of injecting HttpClient.
8. Integrated Swagger in development env for testing of the application.

To Run the application:
The application needs to be configured to start with multiple projects as shown below.

![Capture](https://github.com/rakeshrocket007/EmployeeTasks/assets/10659108/b1defbe7-6c38-46f5-8902-cdea758464d7)


