namespace TeachersAffairs.Application.Common.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ITeacherRepository Teachers { get; }

    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : Domain.Common.BaseEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}