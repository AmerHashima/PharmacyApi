using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Entities.Accounting;
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

    // Reference Data
    public DbSet<GenericName> GenericNames { get; set; }

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
    public DbSet<Customer> Customers { get; set; }

    // Returns & Refunds
    public DbSet<ReturnInvoice> ReturnInvoices { get; set; }
    public DbSet<ReturnInvoiceItem> ReturnInvoiceItems { get; set; }
    public DbSet<InvoiceShape> InvoiceShapes { get; set; }
    public DbSet<InvoiceSetup> InvoiceSetups { get; set; }

    // Integrations
    public DbSet<IntegrationProvider> IntegrationProviders { get; set; }
    public DbSet<BranchIntegrationSetting> BranchIntegrationSettings { get; set; }

    // Stock Transaction Returns
    public DbSet<StockTransactionReturn> StockTransactionReturns { get; set; }
    public DbSet<StockTransactionReturnDetail> StockTransactionReturnDetails { get; set; }

    // RSD Operation Logs
    public DbSet<RsdOperationLog> RsdOperationLogs { get; set; }
    public DbSet<RsdOperationLogDetail> RsdOperationLogDetails { get; set; }

    public DbSet<Link> Links { get; set; }
    public DbSet<ReportParameter> ReportParameters { get; set; }

    // Doctors
    public DbSet<Doctor> Doctors { get; set; }

    // Offers
    public DbSet<OfferMaster> OfferMasters { get; set; }
    public DbSet<OfferDetail> OfferDetails { get; set; }

    // Accounting
    public DbSet<FiscalYear> FiscalYears { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<CostCenter> CostCenters { get; set; }
    public DbSet<CashBox> CashBoxes { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<JournalEntryDetail> JournalEntryDetails { get; set; }
    public DbSet<ReceiptVoucher> ReceiptVouchers { get; set; }
    public DbSet<ReceiptVoucherDetail> ReceiptVoucherDetails { get; set; }
    public DbSet<PaymentVoucher> PaymentVouchers { get; set; }
    public DbSet<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }

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

        // 🔹 Explicitly configure the two SystemUser → Branch relationships
        //     so EF Core does not try to auto-resolve a single inverse collection.
        modelBuilder.Entity<SystemUser>()
            .HasOne(u => u.Branch)
            .WithMany()
            .HasForeignKey(u => u.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SystemUser>()
            .HasOne(u => u.DefaultBranch)
            .WithMany()
            .HasForeignKey(u => u.DefaultBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 InvoiceSetup → Branch (nullable FK, no cascade)
        modelBuilder.Entity<InvoiceSetup>()
            .HasOne(i => i.Branch)
            .WithMany(b => b.InvoiceSetups)
            .HasForeignKey(i => i.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 InvoiceSetup → AppLookupDetail (nullable FK, no cascade)
        modelBuilder.Entity<InvoiceSetup>()
            .HasOne(i => i.InvoiceType)
            .WithMany()
            .HasForeignKey(i => i.InvoiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.GTIN)
            .IsUnique()
            .HasFilter("[GTIN] IS NOT NULL AND [IsDeleted] = 0");

        // 🔹 Product → GenericName (nullable FK, no cascade)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.GenericNameRef)
            .WithMany(g => g.Products)
            .HasForeignKey(p => p.GenericNameId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 OfferDetail → Product (two FKs — restrict both to avoid cascade conflicts)
        modelBuilder.Entity<OfferDetail>()
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(od => od.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OfferDetail>()
            .HasOne(od => od.FreeProduct)
            .WithMany()
            .HasForeignKey(od => od.FreeProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 SalesInvoiceItem → OfferDetail (nullable FK, no cascade)
        modelBuilder.Entity<SalesInvoiceItem>()
            .HasOne(i => i.OfferDetail)
            .WithMany()
            .HasForeignKey(i => i.OfferDetailId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 ReportParameter → Link (no cascade)
        modelBuilder.Entity<ReportParameter>()
            .HasOne(p => p.Link)
            .WithMany(l => l.ReportParameters)
            .HasForeignKey(p => p.LinksOid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        // 🔹 SalesInvoice → Customer (nullable FK, no cascade)
        modelBuilder.Entity<SalesInvoice>()
            .HasOne(i => i.Customer)
            .WithMany(c => c.SalesInvoices)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

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

        // =========================================
        // 🔹 Accounting module FK configurations
        // =========================================

        // Account self-referencing parent
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Parent)
            .WithMany(a => a.Children)
            .HasForeignKey(a => a.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Account>()
            .HasOne(a => a.FinalAccount)
            .WithMany()
            .HasForeignKey(a => a.FinalAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountCode)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        // CostCenter self-referencing parent
        modelBuilder.Entity<CostCenter>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        // CashBox FKs
        modelBuilder.Entity<CashBox>()
            .HasOne(cb => cb.Branch)
            .WithMany()
            .HasForeignKey(cb => cb.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashBox>()
            .HasOne(cb => cb.Account)
            .WithMany()
            .HasForeignKey(cb => cb.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // BankAccount FKs
        modelBuilder.Entity<BankAccount>()
            .HasOne(ba => ba.Branch)
            .WithMany()
            .HasForeignKey(ba => ba.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BankAccount>()
            .HasOne(ba => ba.Account)
            .WithMany()
            .HasForeignKey(ba => ba.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BankAccount>()
            .HasOne(ba => ba.CurrencyCode)
            .WithMany()
            .HasForeignKey(ba => ba.CurrencyCodeId)
            .OnDelete(DeleteBehavior.Restrict);

        // JournalEntry FKs
        modelBuilder.Entity<JournalEntry>()
            .HasIndex(j => j.EntryNumber)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<JournalEntry>()
            .HasOne(j => j.FiscalYear)
            .WithMany()
            .HasForeignKey(j => j.FiscalYearId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntry>()
            .HasOne(j => j.Branch)
            .WithMany()
            .HasForeignKey(j => j.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntry>()
            .HasOne(j => j.ReferenceType)
            .WithMany()
            .HasForeignKey(j => j.ReferenceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntry>()
            .Property(j => j.TotalDebit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JournalEntry>()
            .Property(j => j.TotalCredit)
            .HasPrecision(18, 2);

        // JournalEntryDetail FKs
        modelBuilder.Entity<JournalEntryDetail>()
            .HasOne(d => d.JournalEntry)
            .WithMany(j => j.Details)
            .HasForeignKey(d => d.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JournalEntryDetail>()
            .HasOne(d => d.Account)
            .WithMany()
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntryDetail>()
            .HasOne(d => d.CostCenter)
            .WithMany()
            .HasForeignKey(d => d.CostCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntryDetail>()
            .Property(d => d.Debit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JournalEntryDetail>()
            .Property(d => d.Credit)
            .HasPrecision(18, 2);

        // ReceiptVoucher FKs
        modelBuilder.Entity<ReceiptVoucher>()
            .HasOne(rv => rv.FiscalYear)
            .WithMany()
            .HasForeignKey(rv => rv.FiscalYearId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptVoucher>()
            .HasOne(rv => rv.Branch)
            .WithMany()
            .HasForeignKey(rv => rv.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptVoucher>()
            .HasOne(rv => rv.CashBox)
            .WithMany()
            .HasForeignKey(rv => rv.CashBoxId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptVoucher>()
            .HasOne(rv => rv.BankAccount)
            .WithMany()
            .HasForeignKey(rv => rv.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptVoucher>()
            .HasOne(rv => rv.JournalEntry)
            .WithMany()
            .HasForeignKey(rv => rv.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptVoucher>()
            .Property(rv => rv.TotalAmount)
            .HasPrecision(18, 2);

        // ReceiptVoucherDetail FKs
        modelBuilder.Entity<ReceiptVoucherDetail>()
            .HasOne(d => d.ReceiptVoucher)
            .WithMany(rv => rv.Details)
            .HasForeignKey(d => d.ReceiptVoucherId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReceiptVoucherDetail>()
            .HasOne(d => d.Account)
            .WithMany()
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptVoucherDetail>()
            .HasOne(d => d.CostCenter)
            .WithMany()
            .HasForeignKey(d => d.CostCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReceiptVoucherDetail>()
            .Property(d => d.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ReceiptVoucherDetail>()
            .HasOne(d => d.Customer)
            .WithMany()
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // PaymentVoucher FKs
        modelBuilder.Entity<PaymentVoucher>()
            .HasOne(pv => pv.FiscalYear)
            .WithMany()
            .HasForeignKey(pv => pv.FiscalYearId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentVoucher>()
            .HasOne(pv => pv.Branch)
            .WithMany()
            .HasForeignKey(pv => pv.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentVoucher>()
            .HasOne(pv => pv.CashBox)
            .WithMany()
            .HasForeignKey(pv => pv.CashBoxId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentVoucher>()
            .HasOne(pv => pv.BankAccount)
            .WithMany()
            .HasForeignKey(pv => pv.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentVoucher>()
            .HasOne(pv => pv.JournalEntry)
            .WithMany()
            .HasForeignKey(pv => pv.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentVoucher>()
            .Property(pv => pv.TotalAmount)
            .HasPrecision(18, 2);

        // PaymentVoucherDetail FKs
        modelBuilder.Entity<PaymentVoucherDetail>()
            .HasOne(d => d.PaymentVoucher)
            .WithMany(pv => pv.Details)
            .HasForeignKey(d => d.PaymentVoucherId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PaymentVoucherDetail>()
            .HasOne(d => d.Account)
            .WithMany()
            .HasForeignKey(d => d.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentVoucherDetail>()
            .HasOne(d => d.CostCenter)
            .WithMany()
            .HasForeignKey(d => d.CostCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentVoucherDetail>()
            .Property(d => d.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaymentVoucherDetail>()
            .HasOne(d => d.Stakeholder)
            .WithMany()
            .HasForeignKey(d => d.StakeholderId)
            .OnDelete(DeleteBehavior.Restrict);
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