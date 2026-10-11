using System.ComponentModel.DataAnnotations;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.ViewModels.Projects;

public class ProjectCreateViewModel
{
    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; } = true;

    public List<int> SelectedEmployeeIds { get; set; }
        = new();

    public List<Employee> Employees { get; set; }
        = new();
}