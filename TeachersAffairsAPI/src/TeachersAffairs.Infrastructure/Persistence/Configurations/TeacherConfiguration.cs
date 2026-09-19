using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeachersAffairs.Domain.Entities;

namespace TeachersAffairs.Infrastructure.Persistence.Configurations;

internal sealed class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(t => t.LastName).IsRequired().HasMaxLength(100);

        builder.Property(t => t.EmployeeNumber).IsRequired().HasMaxLength(20);
        builder.Property(t => t.NationalId).IsRequired().HasMaxLength(20);
        builder.Property(t => t.Email).IsRequired().HasMaxLength(200);
        builder.Property(t => t.PhoneNumber).HasMaxLength(20);
        builder.Property(t => t.Department).IsRequired().HasMaxLength(100);

        builder.Property(t => t.Salary).HasColumnType("decimal(18,2)");

        builder.Property(t => t.Gender)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.Level)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.DateOfBirth).HasColumnType("date");
        builder.Property(t => t.HireDate).HasColumnType("date");

        builder.Property(t => t.IsActive).HasDefaultValue(true);

        builder.Ignore(t => t.FullName);

        builder.HasIndex(t => t.Email).IsUnique();
        builder.HasIndex(t => t.EmployeeNumber).IsUnique();
        builder.HasIndex(t => t.NationalId).IsUnique();
        builder.HasIndex(t => t.Department);
        builder.HasIndex(t => t.LastName);
    }
}