
namespace WorkFlowHub.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string?Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime?EndDate { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
                = new List<EmployeeProject>();
    }
    
    
}
