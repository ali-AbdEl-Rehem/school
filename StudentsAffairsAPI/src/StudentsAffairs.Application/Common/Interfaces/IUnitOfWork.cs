namespace StudentsAffairs.Application.Common.Interfaces;

/// <summary>
/// Coordinates one business transaction: hands out the repositories that share a
/// single change-tracking context and commits them together with <see cref="SaveChangesAsync"/>.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    IStudentRepository Students { get; }

    /// <summary>Access any aggregate through the generic repository without a dedicated interface.</summary>
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : Domain.Common.BaseEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
