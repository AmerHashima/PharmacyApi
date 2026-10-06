using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.UnitTests;

public class StockRepositoryTests
{
    [Fact]
    public async Task DeductAsync_WithoutBatch_UsesFefoAndSkipsExpiredStock()
    {
        await using var context = CreateContext();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var today = DateTime.UtcNow.Date;

        context.Stocks.AddRange(
            Stock(productId, branchId, "EXPIRED", today.AddDays(-1), 100),
            Stock(productId, branchId, "EARLY", today.AddDays(10), 3),
            Stock(productId, branchId, "LATE", today.AddDays(30), 5));
        await context.SaveChangesAsync();

        var allocations = await new StockRepository(context)
            .DeductAsync(productId, branchId, 6);

        allocations.Select(x => (x.BatchNumber, x.Quantity)).Should().Equal(
            ("EARLY", 3m),
            ("LATE", 3m));
        (await context.Stocks.SingleAsync(x => x.BatchNumber == "EXPIRED")).Quantity.Should().Be(100);
        (await context.Stocks.SingleAsync(x => x.BatchNumber == "EARLY")).Quantity.Should().Be(0);
        (await context.Stocks.SingleAsync(x => x.BatchNumber == "LATE")).Quantity.Should().Be(2);
    }

    [Fact]
    public async Task DeductAsync_WhenTotalIsInsufficient_DoesNotChangeStock()
    {
        await using var context = CreateContext();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        context.Stocks.Add(Stock(productId, branchId, "B1", DateTime.UtcNow.Date.AddDays(10), 2));
        await context.SaveChangesAsync();

        var action = () => new StockRepository(context).DeductAsync(productId, branchId, 3);

        await action.Should().ThrowAsync<InvalidOperationException>();
        (await context.Stocks.SingleAsync()).Quantity.Should().Be(2);
    }

    [Fact]
    public async Task UpdateQuantityAsync_DoesNotSilentlyClampNegativeStockToZero()
    {
        await using var context = CreateContext();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        context.Stocks.Add(Stock(productId, branchId, "B1", DateTime.UtcNow.Date.AddDays(10), 2));
        await context.SaveChangesAsync();

        var action = () => new StockRepository(context)
            .UpdateQuantityAsync(productId, branchId, -3, "B1");

        await action.Should().ThrowAsync<InvalidOperationException>();
        (await context.Stocks.SingleAsync()).Quantity.Should().Be(2);
    }

    [Fact]
    public async Task ReceiveAsync_RecalculatesWeightedAverageCost()
    {
        await using var context = CreateContext();
        var productId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var existing = Stock(productId, branchId, "B1", DateTime.UtcNow.Date.AddDays(30), 10);
        existing.AverageCost = 5;
        context.Stocks.Add(existing);
        await context.SaveChangesAsync();

        var result = await new StockRepository(context).ReceiveAsync(
            productId, branchId, 10, 7, "B1", DateTime.UtcNow.Date.AddDays(30));

        result.Quantity.Should().Be(20);
        result.AverageCost.Should().Be(6);
    }

    private static PharmacyDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PharmacyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new PharmacyDbContext(options);
    }

    private static Stock Stock(
        Guid productId,
        Guid branchId,
        string batch,
        DateTime expiry,
        decimal quantity) => new()
        {
            ProductId = productId,
            BranchId = branchId,
            BatchNumber = batch,
            ExpiryDate = expiry,
            Quantity = quantity,
            ReservedQuantity = 0
        };
}
