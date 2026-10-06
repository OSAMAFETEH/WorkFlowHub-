using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkFlowHub.Application.Common;
using WorkFlowHub.Application.DTOs.Employees;
using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employee_Repo;
    private readonly IDepartmentRepository _department_Repo;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository,ILogger<EmployeeService> logger)
    {
        _employee_Repo = employeeRepository;
        _department_Repo = departmentRepository;
        _logger = logger;
    }

    public async Task<PagedResult<EmployeeDto>> SearchAsync(
        EmployeeQueryDto request)
    {
        var query = _employee_Repo.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(e =>
                e.FullName.Contains(search) ||
                e.Email.Contains(search));
        }

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(e =>
                e.DepartmentId == request.DepartmentId.Value);
        }

        query = request.SortBy?.ToLower() switch
        {
            "email" => request.SortDescending
                ? query.OrderByDescending(e => e.Email)
                : query.OrderBy(e => e.Email),

            "hiredate" => request.SortDescending
                ? query.OrderByDescending(e => e.HireDate)
                : query.OrderBy(e => e.HireDate),

            _ => request.SortDescending
                ? query.OrderByDescending(e => e.FullName)
                : query.OrderBy(e => e.FullName)
        };

        var totalCount = await query.CountAsync();

        var page = Math.Max(request.Page, 1);

        var pageSize = Math.Clamp(
            request.PageSize,
            1,
            100);

        var employees = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                HireDate = e.HireDate,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.Name
            })
            .ToListAsync();

        return new PagedResult<EmployeeDto>
        {
            Items = employees,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
    public async Task<(bool Success, string? Error)>CreateAsync(CreateEmployeeDto dto)
    {
        var email = dto.Email.ToLowerInvariant().Trim();
        if(await _employee_Repo.ExistsByEmailAsync(email))
        {
            _logger.LogWarning("Employee creation failed.Duplicate email: {Email}", email);
            return (false, "Email is already registered.");
        }
        if(!await _department_Repo.ExistAsync(dto.DepartmentId))
        {
            _logger.LogWarning("Employee creataion failed.Department not found.DepartmentId: {DepartmentId}", dto.DepartmentId);
            return (false, "Selected department does not exist.");
        }
        var employee = new Employee
        {
            FullName = dto.FullName.Trim(),
            HireDate = dto.HireDate,
            Email = email,
            DepartmentId=dto.DepartmentId
        };
        await _employee_Repo.AddAsync(employee);
        await _employee_Repo.SaveChangesAsync();
        _logger.LogInformation("Employee created successfully. EmployeeId: {EmployeeId} ,Email: {Email}", employee.Id, employee.Email);
        return (true, null);

    }
    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _employee_Repo.GetByIdAsync(id);

        if (employee is null)
            return null;

        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            HireDate = employee.HireDate,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department.Name ?? "No Department"
        };
    }
    public async Task<(bool Success, string? Error)> UpdateAsync(
    int id,
    UpdateEmployeeDto dto)
    {
        var employee =
            await _employee_Repo.GetByIdAsync(id);

        if (employee is null)
        {
            return (false, "Employee not found.");
        }

        if (!await _department_Repo
            .ExistAsync(dto.DepartmentId))
        {
            return (false, "Selected department does not exist.");
        }

        var email = dto.Email
            .Trim()
            .ToLowerInvariant();

        if (await _employee_Repo
            .ExistsByEmailAsync(email, id))
        {
            return (false, "Email is already registered.");
        }

        employee.FullName = dto.FullName.Trim();
        employee.Email = email;
        employee.HireDate = dto.HireDate;
        employee.DepartmentId = dto.DepartmentId;

        _employee_Repo.Update(employee);

        await _employee_Repo.SaveChangesAsync();

        return (true, null);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var employee =
            await _employee_Repo.GetByIdAsync(id);

        if (employee is null)
            return false;

        _employee_Repo.Delete(employee);

        await _employee_Repo.SaveChangesAsync();

        return true;
    }
}