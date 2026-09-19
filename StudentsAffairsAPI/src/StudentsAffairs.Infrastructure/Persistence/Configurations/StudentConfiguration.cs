using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentsAffairs.Domain.Entities;

namespace StudentsAffairs.Infrastructure.Persistence.Configurations;

internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.LastName).IsRequired().HasMaxLength(100);

        builder.Property(s => s.StudentNumber).IsRequired().HasMaxLength(20);
        builder.Property(s => s.NationalId).IsRequired().HasMaxLength(20);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(200);
        builder.Property(s => s.PhoneNumber).HasMaxLength(20);
        builder.Property(s => s.Department).IsRequired().HasMaxLength(100);

        builder.Property(s => s.Gpa).HasColumnType("decimal(3,2)");

        builder.Property(s => s.Gender)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.Level)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.DateOfBirth).HasColumnType("date");
        builder.Property(s => s.EnrollmentDate).HasColumnType("date");

        builder.Property(s => s.IsActive).HasDefaultValue(true);

        builder.Ignore(s => s.FullName);

        builder.HasIndex(s => s.Email).IsUnique();
        builder.HasIndex(s => s.StudentNumber).IsUnique();
        builder.HasIndex(s => s.NationalId).IsUnique();
        builder.HasIndex(s => s.Department);
        builder.HasIndex(s => s.LastName);
    }
}
