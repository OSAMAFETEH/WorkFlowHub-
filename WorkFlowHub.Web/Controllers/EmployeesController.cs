using Microsoft.AspNetCore.Mvc;
using WorkFlowHub.Application.DTOs.Employees;
using WorkFlowHub.Application.Services;

namespace WorkFlowHub.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly EmployeeService _service;

    public EmployeesController(
        EmployeeService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(
        EmployeeQueryDto request)
    {
        var result =
            await _service.SearchAsync(request);

        return View(result);
    }
}