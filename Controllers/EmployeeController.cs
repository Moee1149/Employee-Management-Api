using Microsoft.AspNetCore.Mvc;
using MyMvcApp.IService;
using MyWebApiApp.Dto;

namespace MyMvcApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult> getAllEmployees(string search = "", int page = 1)
    {
        int pageSize = 3;

        var (employee, totalCount) = await _employeeService.GetAllEmployee(search, pageSize, page);
        return Ok(new
        {
            Employees = employee,
            TotalCount = totalCount,
            PageSize = pageSize
        });
    }

    [HttpPost]
    public async Task<IActionResult> createNewEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
    {
        if (ModelState.IsValid)
        {
            await _employeeService.CreateNewEmployee(employeeCreateDto);
            return Ok(new
            {
                message = "Employee Created Successfully!"
            });
        }
        return BadRequest(new { message = "Invalid data." });
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult> getEmployeeById(int id)
    {
        EmployeeDto employeeUpdateDto = await _employeeService.GetEmployeeById(id);
        return Ok(new
        {
            message = "Request Successfull",
            data = employeeUpdateDto
        });
    }

    [HttpPut("edit/{id}")]
    public async Task<ActionResult> updateEmployeeData([FromBody] EmployeeUpdateDto employeeUpdateDto)
    {
        await _employeeService.UpdateEmployee(employeeUpdateDto);
        return Ok(new { message = "Employee Update Successfully!" });
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        await _employeeService.DeleteEmployee(id);
        return Ok(new { message = "Employee Deleted Successfully!" });
    }
}

