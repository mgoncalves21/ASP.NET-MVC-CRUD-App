using EmployeesListMVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeesListMVC.Data;

public class MvcDbContext : DbContext
{
    public MvcDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Employee> Employees { get; set; }  

}