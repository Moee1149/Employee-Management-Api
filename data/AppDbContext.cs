using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Models; // Namespace where your models live

namespace MyWebApiApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Example table
    public DbSet<Employee> Employees { get; set; }
    public DbSet<User> Users { get; set; }
}
