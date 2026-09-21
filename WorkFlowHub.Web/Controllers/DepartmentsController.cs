using Microsoft.AspNetCore.Mvc;
using WorkFlowHub.Application.Services;

namespace WorkFlowHub.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly DepartmentService _service;

    public DepartmentsController(DepartmentService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var departments = await _service.GetAllAsync();

        return View(departments);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        string name,
        string? description)
    {
        var result = await _service.CreateAsync(
            name,
            description);

        if (!result.Success)
        {
            ModelState.AddModelError(
                "Name",
                result.Error!);

            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}