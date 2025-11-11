using MyWebApiApp.Dto;
using MyWebApiApp.Models;

namespace MyMvcApp.IService;

public interface IEmployeeService
{
    public Task<(List<EmployeeDto>, int)> GetAllEmployee(string search, int pageSize, int pageNumber);
    public Task CreateNewEmployee(EmployeeCreateDto employee);
    public Task UpdateEmployee(EmployeeUpdateDto employee);
    public Task DeleteEmployee(int employeeId);
    public Task<EmployeeDto> GetEmployeeById(int employeeId);
}