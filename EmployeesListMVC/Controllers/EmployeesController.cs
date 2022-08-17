using Microsoft.EntityFrameworkCore;
using EmployeesListMVC.Data;
using EmployeesListMVC.Models;
using EmployeesListMVC.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesListMVC.Controllers;

public class Employees : Controller
{
    private readonly MvcDbContext _mvcDbContext;

    public Employees(MvcDbContext mvcDbContext)
    {
        _mvcDbContext = mvcDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var employee = await _mvcDbContext.Employees.ToListAsync();
        return View(employee);
    }

    // GET ADD PAGE
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    //POST FORM ADD EMPLOYEE
    [HttpPost]
    public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
    {
        var employee = new Employee()
        {
            Id = Guid.NewGuid(),
            Name = addEmployeeRequest.Name,
            Department = addEmployeeRequest.Department,
            Email = addEmployeeRequest.Email,
            DateOfBirth = addEmployeeRequest.DateOfBirth,
            Salary = addEmployeeRequest.Salary
        };
        await _mvcDbContext.Employees.AddAsync(employee);
        await _mvcDbContext.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    //GET EDIT PAGE
    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        var employee = await _mvcDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null) return RedirectToAction("Index");

        var viewModel = new UpdateEmployeeViewModel()
        {
            Id = Guid.NewGuid(),
            Name = employee!.Name,
            Department = employee.Department,
            Email = employee.Email,
            DateOfBirth = employee.DateOfBirth,
            Salary = employee.Salary
        };
        return await Task.Run(() => View ("View", viewModel)) ;
    }
    //POST EDIT UPDATE
    [HttpPost]
    public async Task<IActionResult> View(UpdateEmployeeViewModel model)
    {
        var employee = await _mvcDbContext.Employees.FindAsync(model.Id);

        if (employee == null) return RedirectToAction("Index");

        Guid.NewGuid();
        employee.Name = model.Name;
        employee.Department = model.Department;
        employee.Email = model.Email;
        employee.DateOfBirth = model.DateOfBirth;
        employee.Salary = model.Salary;

        await _mvcDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
    {
        var employee = await _mvcDbContext.Employees.FindAsync(model.Id);
        if (employee != null)
        {
             _mvcDbContext.Employees.Remove(employee);
             await _mvcDbContext.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}