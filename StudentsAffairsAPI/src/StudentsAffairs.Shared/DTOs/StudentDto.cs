using StudentsAffairs.Domain.Enums;

namespace StudentsAffairs.Shared.DTOs;

/// <summary>Read model returned to the presentation layer / API consumers.</summary>
public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public AcademicLevel Level { get; set; }
    public string Department { get; set; } = string.Empty;
    public decimal Gpa { get; set; }
    public DateOnly EnrollmentDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
