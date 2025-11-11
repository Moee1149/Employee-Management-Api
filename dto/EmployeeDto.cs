namespace MyWebApiApp.Dto;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Department { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime HireDate { get; set; }
}