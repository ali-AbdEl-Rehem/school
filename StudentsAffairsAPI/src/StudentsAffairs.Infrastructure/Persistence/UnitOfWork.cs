using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage;
using StudentsAffairs.Application.Common.Interfaces;
using StudentsAffairs.Domain.Common;
using StudentsAffairs.Infrastructure.Persistence.Repositories;

namespace StudentsAffairs.Infrastructure.Persistence;

/// <summary>
/// EF Core Unit of Work. Owns one <see cref="ApplicationDbContext"/> for the scope,
/// lazily exposes repositories that all share it, and commits them atomically.
/// The DI container owns the context lifetime; this type only disposes an open transaction.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    private IStudentRepository? _students;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context) => _context = context;

    public IStudentRepository Students => _students ??= new StudentRepository(_context);

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        => (IGenericRepository<TEntity>)_repositories.GetOrAdd(
            typeof(TEntity),
            _ => new GenericRepository<TEntity>(_context));

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
