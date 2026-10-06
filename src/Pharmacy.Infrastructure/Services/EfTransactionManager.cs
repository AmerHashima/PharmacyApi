using System.Data;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Common.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Services;

public sealed class EfTransactionManager : ITransactionManager
{
    private readonly PharmacyDbContext _context;

    public EfTransactionManager(PharmacyDbContext context)
    {
        _context = context;
    }

    public async Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        if (_context.Database.CurrentTransaction != null)
            return await operation(cancellationToken);

        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable, cancellationToken);
            try
            {
                var result = await operation(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}

