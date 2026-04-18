using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions;

namespace Pharmacy.Infrastructure.Persistence;

public class PharmacyDbContext : DbContext
{
    public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options)
    {
    }

    // Core System Tables
    public DbSet<SystemUser> SystemUsers { get; set; }
    public DbSet<Role> Roles { get; set; }

    // Lookups
    public DbSet<AppLookupMaster> AppLookupMasters { get; set; }
    public DbSet<AppLookupDetail> AppLookupDetails { get; set; }

    // Pharmacy Structure
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Stakeholder> Stakeholders { get; set; }
    public DbSet<StakeholderBranch> StakeholderBranches { get; set; }
    public DbSet<Store> Stores { get; set; }

    // Products & Inventory
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductBatch> ProductBatches { get; set; }
    public DbSet<ProductUnit> ProductUnits { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }
    public DbSet<StockTransactionDetail> StockTransactionDetails { get; set; }

    // Sales & POS
    public DbSet<SalesInvoice> SalesInvoices { get; set; }
    public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }

    // Returns & Refunds
    public DbSet<ReturnInvoice> ReturnInvoices { get; set; }
    public DbSet<ReturnInvoiceItem> ReturnInvoiceItems { get; set; }
    public DbSet<InvoiceShape> InvoiceShapes { get; set; }

    // Integrations
    public DbSet<IntegrationProvider> IntegrationProviders { get; set; }
    public DbSet<BranchIntegrationSetting> BranchIntegrationSettings { get; set; }

    // Stock Transaction Returns
    public DbSet<StockTransactionReturn> StockTransactionReturns { get; set; }
    public DbSet<StockTransactionReturnDetail> StockTransactionReturnDetails { get; set; }

    // RSD Operation Logs
    public DbSet<RsdOperationLog> RsdOperationLogs { get; set; }
    public DbSet<RsdOperationLogDetail> RsdOperationLogDetails { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔹 Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // 🔹 Configure unique indexes
        modelBuilder.Entity<Branch>()
            .HasIndex(b => b.BranchCode)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Store>()
            .HasIndex(s => s.StoreCode)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Branch>()
            .HasOne(b => b.DefaultStore)
            .WithMany()
            .HasForeignKey(b => b.DefaultStoreId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.GTIN)
            .IsUnique()
            .HasFilter("[GTIN] IS NOT NULL AND [IsDeleted] = 0");

        modelBuilder.Entity<SalesInvoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        // 🔹 Configure relationships for StockTransaction
        modelBuilder.Entity<StockTransaction>()
            .HasOne(t => t.FromBranch)
            .WithMany(b => b.OutgoingTransactions)
            .HasForeignKey(t => t.FromBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>()
            .HasOne(t => t.ToBranch)
            .WithMany(b => b.IncomingTransactions)
            .HasForeignKey(t => t.ToBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>()
            .HasOne(t => t.Store)
            .WithMany()
            .HasForeignKey(t => t.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 Configure decimal precision
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        // 🔹 Unique index: Stock is tracked per Product + Branch + BatchNumber
        modelBuilder.Entity<Stock>()
            .HasIndex(s => new { s.ProductId, s.BranchId, s.BatchNumber })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Stock>()
            .Property(s => s.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Stock>()
            .Property(s => s.ReservedQuantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StockTransactionDetail>()
            .Property(t => t.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoice>()
            .Property(i => i.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(i => i.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(i => i.TotalPrice)
            .HasPrecision(18, 2);

        // 🔹 Configure ReturnInvoice unique index
        modelBuilder.Entity<ReturnInvoice>()
            .HasIndex(i => i.ReturnNumber)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<ReturnInvoice>()
            .Property(i => i.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ReturnInvoice>()
            .Property(i => i.RefundAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ReturnInvoice>()
            .HasOne(r => r.OriginalInvoice)
            .WithMany(s => s.ReturnInvoices)
            .HasForeignKey(r => r.OriginalInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReturnInvoiceItem>()
            .Property(i => i.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ReturnInvoiceItem>()
            .Property(i => i.TotalPrice)
            .HasPrecision(18, 2);

        // 🔹 Configure relationships for StockTransactionReturn
        modelBuilder.Entity<StockTransactionReturn>()
            .HasOne(t => t.FromBranch)
            .WithMany()
            .HasForeignKey(t => t.FromBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransactionReturn>()
            .HasOne(t => t.ToBranch)
            .WithMany()
            .HasForeignKey(t => t.ToBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransactionReturn>()
            .HasOne(t => t.OriginalTransaction)
            .WithMany()
            .HasForeignKey(t => t.OriginalTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransactionReturn>()
            .HasOne(t => t.ReturnInvoice)
            .WithMany(r => r.StockTransactionReturns)
            .HasForeignKey(t => t.ReturnInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 Configure decimal precision for StockTransactionReturn
        modelBuilder.Entity<StockTransactionReturn>()
            .Property(t => t.TotalValue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StockTransactionReturnDetail>()
            .Property(d => d.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StockTransactionReturnDetail>()
            .Property(d => d.UnitCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StockTransactionReturnDetail>()
            .Property(d => d.TotalCost)
            .HasPrecision(18, 2);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    // You can set CreatedBy here if you have user context
                    // entry.Entity.CreatedBy = _currentUserService.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    // You can set UpdatedBy here if you have user context
                    // entry.Entity.UpdatedBy = _currentUserService.UserId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}