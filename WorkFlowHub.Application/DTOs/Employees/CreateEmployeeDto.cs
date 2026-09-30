using System.ComponentModel.DataAnnotations;
namespace WorkFlowHub.Application.DTOs.Employees
{
    public  class CreateEmployeeDto
    {
        [Required]
        [StringLength(150,MinimumLength=2)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public int DepartmentId { get; set; }
    }
}
