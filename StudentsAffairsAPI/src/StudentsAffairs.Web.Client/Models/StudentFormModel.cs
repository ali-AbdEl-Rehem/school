using System.ComponentModel.DataAnnotations;
using StudentsAffairs.Domain.Enums;
using StudentsAffairs.Shared.DTOs;

namespace StudentsAffairs.Web.Client.Models;

/// <summary>Single editable model backing both the Create and Edit forms.</summary>
public class StudentFormModel
{
    public int Id { get; set; }

    [Required, StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "Only letters, digits and dashes are allowed.")]
    public string StudentNumber { get; set; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 5)]
    public string NationalId { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; } = new(2005, 1, 1);

    [Range(1, 2, ErrorMessage = "Please choose a gender.")]
    public Gender Gender { get; set; } = Gender.Unspecified;

    [Required]
    public AcademicLevel Level { get; set; } = AcademicLevel.Freshman;

    [Required, StringLength(100, MinimumLength = 2)]
    public string Department { get; set; } = string.Empty;

    [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.00 and 4.00.")]
    public decimal Gpa { get; set; }

    [Required]
    public DateOnly EnrollmentDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public bool IsActive { get; set; } = true;

    public static StudentFormModel FromDto(StudentDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        StudentNumber = dto.StudentNumber,
        NationalId = dto.NationalId,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        DateOfBirth = dto.DateOfBirth,
        Gender = dto.Gender,
        Level = dto.Level,
        Department = dto.Department,
        Gpa = dto.Gpa,
        EnrollmentDate = dto.EnrollmentDate,
        IsActive = dto.IsActive
    };

    public StudentCreateDto ToCreateDto() => new()
    {
        FirstName = FirstName,
        LastName = LastName,
        StudentNumber = StudentNumber,
        NationalId = NationalId,
        Email = Email,
        PhoneNumber = PhoneNumber,
        DateOfBirth = DateOfBirth,
        Gender = Gender,
        Level = Level,
        Department = Department,
        Gpa = Gpa,
        EnrollmentDate = EnrollmentDate,
        IsActive = IsActive
    };

    public StudentUpdateDto ToUpdateDto() => new()
    {
        Id = Id,
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
        PhoneNumber = PhoneNumber,
        DateOfBirth = DateOfBirth,
        Gender = Gender,
        Level = Level,
        Department = Department,
        Gpa = Gpa,
        EnrollmentDate = EnrollmentDate,
        IsActive = IsActive
    };
}
