
using Microsoft.EntityFrameworkCore;
using WorkFlowHub.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace WorkFlowHub.Infrastructure.Data.Configurations
{
    public class ProjectConfiguration:IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.IsActive) .IsRequired();
            builder.Property(p => p.CreatedAt) .IsRequired();
        }
    }
}
