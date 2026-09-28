
using WorkFlowHub.Application.Common;
using WorkFlowHub.Application.DTOs.Employees;
using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeDto>> SearchAsync(
        EmployeeQueryDto request)
    {
        var query = _repository.Query();

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
}