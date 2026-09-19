using StudentsAffairs.Domain.Common;
using StudentsAffairs.Domain.Enums;

namespace StudentsAffairs.Domain.Entities;

/// <summary>
/// Aggregate root of the "Students" domain — a person enrolled in the institution.
/// </summary>
public class Student : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    /// <summary>Institution-issued identifier, e.g. "STU-2025-0001". Unique.</summary>
    public string StudentNumber { get; set; } = string.Empty;

    /// <summary>National / government identifier. Unique.</summary>
    public string NationalId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public Gender Gender { get; set; } = Gender.Unspecified;

    public AcademicLevel Level { get; set; } = AcademicLevel.Freshman;

    public string Department { get; set; } = string.Empty;

    /// <summary>Grade point average on a 0.00 – 4.00 scale.</summary>
    public decimal Gpa { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    public bool IsActive { get; set; } = true;

    // ---- Derived, not persisted ----
    public string FullName => $"{FirstName} {LastName}".Trim();
}
