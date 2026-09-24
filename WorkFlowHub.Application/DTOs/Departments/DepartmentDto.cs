using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowHub.Application.DTOs.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int EmployeeCount { get; set; }
    }
}
