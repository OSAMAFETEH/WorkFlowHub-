using Microsoft.EntityFrameworkCore;
using WorkFlowHub.Application.Common;
using WorkFlowHub.Application.DTOs.Employees;
using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employee_Repo;
    private readonly IDepartmentRepository _department_Repo;

    public EmployeeService(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository)
    {
        _employee_Repo = employeeRepository;
        _department_Repo = departmentRepository;
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
            return (false, "Email is already registered.");
        }
        if(!await _department_Repo.ExistAsync(dto.DepartmentId))
        {
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
        return (true, null);

    }
}