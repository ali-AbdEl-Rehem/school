using TeachersAffairs.Domain.Common;
using TeachersAffairs.Domain.Enums;

namespace TeachersAffairs.Domain.Entities;

public class Teacher : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmployeeNumber { get; set; } = string.Empty;

    public string NationalId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public Gender Gender { get; set; } = Gender.Unspecified;

    public TeacherLevel Level { get; set; } = TeacherLevel.Assistant;

    public string Department { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public DateOnly HireDate { get; set; }

    public bool IsActive { get; set; } = true;

    public string FullName => $"{FirstName} {LastName}".Trim();
}