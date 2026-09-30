using Microsoft.AspNetCore.Mvc;
using WorkFlowHub.Application.DTOs.Employees;
using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Application.Services;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly EmployeeService _service;
    private readonly IDepartmentRepository _departmentRepo;

    public EmployeesController(EmployeeService service,IDepartmentRepository departmentRepo)
    {
        _service = service;
        _departmentRepo = departmentRepo;
    }

    public async Task<IActionResult> Index(
        EmployeeQueryDto request)
    {
        var result =
            await _service.SearchAsync(request);

        return View(result);
    }
    [HttpGet]
    public async Task<IActionResult>Create()
    {
        var department =await _departmentRepo.GetAllAsync();
        ViewBag.Departments = department;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult>Create(CreateEmployeeDto dto)
    {
        if(!ModelState.IsValid)
        {
          await  LoadDepartments();
            View(dto);
        }
        var result =await _service.CreateAsync(dto);
        if(!result.Success)
        {
            ModelState.AddModelError(nameof(dto.Email), result.Error!);
            await LoadDepartments();
            View(dto);
        }
        return RedirectToAction(nameof(Index));

    }
    private async Task LoadDepartments()
    {
        ViewBag.Departments = await _departmentRepo.GetAllAsync();
    }
}