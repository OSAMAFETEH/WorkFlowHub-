

using System.ComponentModel.DataAnnotations;

namespace WorkFlowHub.Application.DTOs.Departments
{
    public  class CreateDepartmentsDto
    {
        [Required]
        [StringLength(100,MinimumLength =2)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Description { get; set; } 

    }
}
