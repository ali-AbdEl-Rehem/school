using System.ComponentModel.DataAnnotations;
using TeachersAffairs.Domain.Enums;

namespace TeachersAffairs.Shared.DTOs;

public class TeacherUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required, StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required, EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    [Required, EnumDataType(typeof(TeacherLevel))]
    public TeacherLevel Level { get; set; }

    [Required, StringLength(100, MinimumLength = 2)]
    public string Department { get; set; } = string.Empty;

    [Range(0.0, double.MaxValue, ErrorMessage = "Salary must be positive.")]
    public decimal Salary { get; set; }

    [Required]
    public DateOnly HireDate { get; set; }

    public bool IsActive { get; set; }
}