
namespace WorkFlowHub.Application.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
            = string.Empty;
    }
}
