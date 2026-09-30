namespace WorkFlowHub.Application.DTOs.Employees;

public class EmployeeQueryDto
{
    public string? Search { get; set; }

    public int? DepartmentId { get; set; }

    public string? SortBy { get; set; }

    public bool SortDescending { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}