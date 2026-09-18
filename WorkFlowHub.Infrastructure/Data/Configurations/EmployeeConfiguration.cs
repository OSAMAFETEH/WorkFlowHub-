using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration:IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FullName).IsRequired().HasMaxLength(150);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(e => e.HireDate).IsRequired();
            builder.HasOne(e => e.Department).WithMany(d => d.Employees)
                .HasForeignKey(E => E.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
                            

        }
    }
}
