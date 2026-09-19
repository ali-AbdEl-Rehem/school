using System.ComponentModel.DataAnnotations;
using TeachersAffairs.Domain.Enums;

namespace TeachersAffairs.Shared.DTOs;

public class TeacherCreateDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "Only letters, digits and dashes are allowed.")]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 5)]
    public string NationalId { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; } = new DateOnly(1980, 1, 1);

    [Required, EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; } = Gender.Unspecified;

    [Required, EnumDataType(typeof(TeacherLevel))]
    public TeacherLevel Level { get; set; } = TeacherLevel.Assistant;

    [Required, StringLength(100, MinimumLength = 2)]
    public string Department { get; set; } = string.Empty;

    [Range(0.0, double.MaxValue, ErrorMessage = "Salary must be positive.")]
    public decimal Salary { get; set; }

    [Required]
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public bool IsActive { get; set; } = true;
}