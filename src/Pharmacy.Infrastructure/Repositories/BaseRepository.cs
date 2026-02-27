using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pharmacy.Domain.Common;
using Pharmacy.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Pharmacy.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly PharmacyDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(PharmacyDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual IQueryable<T> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Oid == id, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => !x.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            // Log the exception (you can use any logging framework)
            Console.WriteLine($"Error adding {typeof(T).Name}: {ex.Message}");
            throw; // Re-throw the exception after logging it
        }
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            await UpdateAsync(entity, cancellationToken);
        }
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(x => x.Oid == id && !x.IsDeleted, cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .CountAsync(x => !x.IsDeleted, cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => !x.IsDeleted)
            .CountAsync(predicate, cancellationToken);
    }
    public async Task<T> InsertMasterDetailAsync<TDetail>(
    T master,
    IEnumerable<TDetail> details,
    CancellationToken cancellationToken = default)
    where TDetail : BaseEntity
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _dbSet.Add(master);
            await _context.SaveChangesAsync(cancellationToken);

            if (details != null && details.Any())
            {
                await _context.Set<TDetail>().AddRangeAsync(details, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return master;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    public async Task UpdateMasterDetailAsync<TDetail>(
    T master,
    IEnumerable<TDetail> details,
    Expression<Func<TDetail, object>> foreignKey,
    CancellationToken cancellationToken = default)
    where TDetail : BaseEntity
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _dbSet.Update(master);

            var detailSet = _context.Set<TDetail>();

            // delete old details
            var masterId = master.Oid;

            var oldDetails = await detailSet
                .Where(x => EF.Property<Guid>(x, foreignKey.GetPropertyAccess().Name) == masterId)
                .ToListAsync(cancellationToken);

            detailSet.RemoveRange(oldDetails);

            if (details != null && details.Any())
            {
                await detailSet.AddRangeAsync(details, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}