using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.IService;
using MyWebApiApp.Dto;
using MyWebApiApp.Iservice;

namespace MyWebApiApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IFileService _fileSevice;
    public EmployeeController(IEmployeeService employeeService, IFileService fileservice)
    {
        _employeeService = employeeService;
        _fileSevice = fileservice;
    }

    [HttpGet]
    public async Task<ActionResult> getAllEmployees(string search = "", int page = 1)
    {
        int pageSize = 3;
        try
        {
            var (employee, totalCount) = await _employeeService.GetAllEmployee(search, pageSize, page);
            return Ok(new
            {
                data = new
                {
                    Employees = employee,
                    TotalCount = totalCount,
                    PageSize = pageSize
                },
                message = "Request Sucessfull"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { message = "An error occured" });
        }
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

    [HttpPut("edit")]
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

    [HttpPost("upload")]
    public async Task<ActionResult> UploadFileToDisk(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file");
        try
        {
            await _fileSevice.SaveFileToDisk(file);
            return Ok("File Uploaded Sucessfllyj");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error uploading file: " + e);
            return StatusCode(500, new { message = "An error occured while uploading the file" });
        }

    }
}

