using TeachersAffairs.Domain.Entities;
using TeachersAffairs.Shared.DTOs;

namespace TeachersAffairs.Application.Teachers.Mapping;

internal static class TeacherMappings
{
    public static TeacherDto ToDto(this Teacher e) => new()
    {
        Id = e.Id,
        FirstName = e.FirstName,
        LastName = e.LastName,
        FullName = e.FullName,
        EmployeeNumber = e.EmployeeNumber,
        NationalId = e.NationalId,
        Email = e.Email,
        PhoneNumber = e.PhoneNumber,
        DateOfBirth = e.DateOfBirth,
        Gender = e.Gender,
        Level = e.Level,
        Department = e.Department,
        Salary = e.Salary,
        HireDate = e.HireDate,
        IsActive = e.IsActive,
        CreatedAtUtc = e.CreatedAtUtc,
        UpdatedAtUtc = e.UpdatedAtUtc
    };

    public static Teacher ToEntity(this TeacherCreateDto d) => new()
    {
        FirstName = d.FirstName.Trim(),
        LastName = d.LastName.Trim(),
        EmployeeNumber = d.EmployeeNumber.Trim(),
        NationalId = d.NationalId.Trim(),
        Email = d.Email.Trim(),
        PhoneNumber = string.IsNullOrWhiteSpace(d.PhoneNumber) ? null : d.PhoneNumber.Trim(),
        DateOfBirth = d.DateOfBirth,
        Gender = d.Gender,
        Level = d.Level,
        Department = d.Department.Trim(),
        Salary = d.Salary,
        HireDate = d.HireDate,
        IsActive = d.IsActive
    };

    public static void Apply(this TeacherUpdateDto d, Teacher e)
    {
        e.FirstName = d.FirstName.Trim();
        e.LastName = d.LastName.Trim();
        e.Email = d.Email.Trim();
        e.PhoneNumber = string.IsNullOrWhiteSpace(d.PhoneNumber) ? null : d.PhoneNumber.Trim();
        e.DateOfBirth = d.DateOfBirth;
        e.Gender = d.Gender;
        e.Level = d.Level;
        e.Department = d.Department.Trim();
        e.Salary = d.Salary;
        e.HireDate = d.HireDate;
        e.IsActive = d.IsActive;
    }
}