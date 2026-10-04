
using System.ComponentModel.DataAnnotations;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.ViewModels.Employees
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public List<Department> Departments { get; set; } = new();
    }
}
