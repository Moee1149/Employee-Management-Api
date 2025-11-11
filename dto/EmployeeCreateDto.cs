namespace MyWebApiApp.Dto;

public class EmployeeCreateDto
{
    public string Name { get; set; } = "";
    public string Department { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime HireDate { get; set; }
}