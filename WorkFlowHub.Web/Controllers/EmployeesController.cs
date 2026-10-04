using Microsoft.AspNetCore.Mvc;
using WorkFlowHub.Application.DTOs.Employees;
using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Application.Services;
using WorkFlowHub.Application.ViewModels.Employees;
using WorkFlowHub.Infrastructure.Repositories;


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
        var model = new EmployeeCreateViewModel
        {
            Departments = department
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult>Create(EmployeeCreateViewModel model)
    {
        if(!ModelState.IsValid)
        {
         model.Departments = await _departmentRepo.GetAllAsync();
            return View(model);
        }
        var dto = new CreateEmployeeDto
        {
            FullName = model.FullName,
            Email = model.Email,
            HireDate = model.HireDate,
            DepartmentId = model.DepartmentId
        };
        var result =await _service.CreateAsync(dto);
        if(!result.Success)
        {
            ModelState.AddModelError(nameof(dto.Email), result.Error!);
            model.Departments =
            await _departmentRepo.GetAllAsync();
          return  View(model);
        }
        return RedirectToAction(nameof(Index));

    }
    [HttpGet]
    public async Task<IActionResult>Edit(int id)
    {
        var employee=await _service.GetByIdAsync(id);
        if (employee is null)
            return NotFound();

        var model = new EmployeeEditViewModel
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            HireDate = employee.HireDate,
            DepartmentId = employee.DepartmentId,

            Departments =
                await _departmentRepo.GetAllAsync()
        };

        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    int id,
    EmployeeEditViewModel model)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            model.Departments =
                await _departmentRepo.GetAllAsync();

            return View(model);
        }

        var dto = new UpdateEmployeeDto
        {
            FullName = model.FullName,
            Email = model.Email,
            HireDate = model.HireDate,
            DepartmentId = model.DepartmentId
        };

        var result = await _service.UpdateAsync(id, dto);

        if (!result.Success)
        {
            if (result.Error == "Employee not found.")
                return NotFound();

            ModelState.AddModelError(
                nameof(model.Email),
                result.Error!);

            model.Departments =
                await _departmentRepo.GetAllAsync();

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}