
using Microsoft.EntityFrameworkCore;
using WorkFlowHub.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace WorkFlowHub.Infrastructure.Data.Configurations
{
    public class EmployeeProjectConfiguration:IEntityTypeConfiguration<EmployeeProject>
    {
        public void Configure(EntityTypeBuilder<EmployeeProject> builder)
        {
            builder.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });
            builder.HasOne(ep => ep.Employee)
                   .WithMany(e => e.EmployeeProjects)
                   .HasForeignKey(ep => ep.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ep => ep.Project)
                   .WithMany(p => p.EmployeeProjects)
                   .HasForeignKey(ep => ep.ProjectId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
