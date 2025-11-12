using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyMvcApp.IService;
using MyWebApiApp.Models;
using MyWebApiApp.Dto;

namespace MyMvcApp.Service;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;
    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateNewEmployee(EmployeeCreateDto employeeDto)
    {
        var employee = new Employee
        {
            Name = employeeDto.Name,
            Department = employeeDto.Department,
            HireDate = employeeDto.HireDate,
            Email = employeeDto.Email
        };
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmployee(int employeeId)
    {
        Employee? employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
        {
            throw new Exception("user not found");
        }
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<(List<EmployeeDto>, int)> GetAllEmployee(string search = "", int pageSize = 3, int pageNumber = 1)
    {
        var query = _context.Employees.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(e =>
            e.Name.ToLower().Contains(search));
        }

        int totalCount = await query.CountAsync();

        var employees = await query
        .OrderBy(e => e.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(e => new EmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            Department = e.Department,
            HireDate = e.HireDate
        })
        .ToListAsync();
        return (employees, totalCount);
    }

    public async Task UpdateEmployee(EmployeeUpdateDto employeeDto)
    {
        var employee = new Employee
        {
            Id = employeeDto.Id,
            Name = employeeDto.Name,
            Department = employeeDto.Department,
            Email = employeeDto.Email
        };
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeDto> GetEmployeeById(int employeeId)
    {
        Employee? employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
        {
            throw new Exception("employee not found");
        }
        return new EmployeeDto
        {
            Name = employee.Name,
            Id = employee.Id,
            Department = employee.Department,
            HireDate = employee.HireDate,
            Email = employee.Email
        };
    }
}