namespace StudentsAffairs.Domain.Common;

/// <summary>
/// Base type for every persistent entity in the domain.
/// Carries the surrogate key and audit timestamps that the
/// infrastructure layer maintains automatically on save.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
