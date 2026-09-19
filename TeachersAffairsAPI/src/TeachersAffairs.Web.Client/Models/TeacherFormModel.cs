using System.ComponentModel.DataAnnotations;
using TeachersAffairs.Domain.Enums;
using TeachersAffairs.Shared.DTOs;

namespace TeachersAffairs.Web.Client.Models;

public class TeacherFormModel
{
    public int Id { get; set; }

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
    public DateOnly DateOfBirth { get; set; } = new(1980, 1, 1);

    [Range(1, 2, ErrorMessage = "Please choose a gender.")]
    public Gender Gender { get; set; } = Gender.Unspecified;

    [Required]
    public TeacherLevel Level { get; set; } = TeacherLevel.Assistant;

    [Required, StringLength(100, MinimumLength = 2)]
    public string Department { get; set; } = string.Empty;

    [Range(0.0, double.MaxValue, ErrorMessage = "Salary must be positive.")]
    public decimal Salary { get; set; }

    [Required]
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public bool IsActive { get; set; } = true;

    public static TeacherFormModel FromDto(TeacherDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        EmployeeNumber = dto.EmployeeNumber,
        NationalId = dto.NationalId,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        DateOfBirth = dto.DateOfBirth,
        Gender = dto.Gender,
        Level = dto.Level,
        Department = dto.Department,
        Salary = dto.Salary,
        HireDate = dto.HireDate,
        IsActive = dto.IsActive
    };

    public TeacherCreateDto ToCreateDto() => new()
    {
        FirstName = FirstName,
        LastName = LastName,
        EmployeeNumber = EmployeeNumber,
        NationalId = NationalId,
        Email = Email,
        PhoneNumber = PhoneNumber,
        DateOfBirth = DateOfBirth,
        Gender = Gender,
        Level = Level,
        Department = Department,
        Salary = Salary,
        HireDate = HireDate,
        IsActive = IsActive
    };

    public TeacherUpdateDto ToUpdateDto() => new()
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
        Salary = Salary,
        HireDate = HireDate,
        IsActive = IsActive
    };
}