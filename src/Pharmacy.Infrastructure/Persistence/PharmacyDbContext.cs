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

    // Products & Inventory
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductBatch> ProductBatches { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }

    // Sales & POS
    public DbSet<SalesInvoice> SalesInvoices { get; set; }
    public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
    // Add these DbSet properties
    public DbSet<IntegrationProvider> IntegrationProviders { get; set; }
    public DbSet<BranchIntegrationSetting> BranchIntegrationSettings { get; set; }
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

        // 🔹 Configure decimal precision
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Stock>()
            .Property(s => s.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Stock>()
            .Property(s => s.ReservedQuantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StockTransaction>()
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