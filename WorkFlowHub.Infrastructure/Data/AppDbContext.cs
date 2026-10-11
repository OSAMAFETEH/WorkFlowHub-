using Microsoft.EntityFrameworkCore;
using WorkFlowHub.Domain.Entities;
namespace WorkFlowHub.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<EmployeeProject> EmployeeProjects => Set<EmployeeProject>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
       typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
