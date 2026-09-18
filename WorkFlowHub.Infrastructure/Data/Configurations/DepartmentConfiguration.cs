using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Infrastructure.Data.Configurations
{
    public  class DepartmentConfiguration: IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Description).HasMaxLength(500);
            builder.Property(d => d.CreatedAt).IsRequired();
        }
    }
}
