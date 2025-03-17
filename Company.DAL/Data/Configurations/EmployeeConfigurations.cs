using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.DAL.Data.Configurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");

            builder.HasOne(E => E.Department)
                   .WithMany(D => D.Employees)
                   .HasForeignKey(E => E.DepartmentId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
