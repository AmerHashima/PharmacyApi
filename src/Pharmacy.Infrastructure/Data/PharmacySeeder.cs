using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Infrastructure.Persistence;
using System.Security.Cryptography;

namespace Pharmacy.Infrastructure.Data;

/// <summary>
/// Seed data for the pharmacy system including Branches, Users, Products, Stock, and Sales.
/// Uses lookup IDs from LookupSeeder for foreign key references.
/// </summary>
public static class PharmacySeeder
{
    // ========================================
    // Role IDs
    // ========================================
    public static readonly Guid RoleAdmin = Guid.Parse("33333333-3333-3333-3333-333333333001");
    public static readonly Guid RoleManager = Guid.Parse("33333333-3333-3333-3333-333333333002");
    public static readonly Guid RoleCashier = Guid.Parse("33333333-3333-3333-3333-333333333003");
    public static readonly Guid RolePharmacist = Guid.Parse("33333333-3333-3333-3333-333333333004");

    // ========================================
    // Branch IDs
    // ========================================
    public static readonly Guid Branch1 = Guid.Parse("44444444-4444-4444-4444-444444444001");
    public static readonly Guid Branch2 = Guid.Parse("44444444-4444-4444-4444-444444444002");

    // ========================================
    // Stakeholder IDs
    // ========================================
    public static readonly Guid PharmacyStakeholder = Guid.Parse("55555555-5555-5555-5555-555555555001");
    public static readonly Guid Supplier1 = Guid.Parse("55555555-5555-5555-5555-555555555002");
    public static readonly Guid Supplier2 = Guid.Parse("55555555-5555-5555-5555-555555555003");

    // ========================================
    // User IDs
    // ========================================
    public static readonly Guid AdminUser1 = Guid.Parse("66666666-6666-6666-6666-666666666001");
    public static readonly Guid AdminUser2 = Guid.Parse("66666666-6666-6666-6666-666666666002");
    public static readonly Guid ManagerUser1 = Guid.Parse("66666666-6666-6666-6666-666666666003");
    public static readonly Guid ManagerUser2 = Guid.Parse("66666666-6666-6666-6666-666666666004");
    public static readonly Guid CashierUser1 = Guid.Parse("66666666-6666-6666-6666-666666666005");
    public static readonly Guid CashierUser2 = Guid.Parse("66666666-6666-6666-6666-666666666006");

    // ========================================
    // Product IDs
    // ========================================
    public static readonly Guid Product1 = Guid.Parse("77777777-7777-7777-7777-777777777001");
    public static readonly Guid Product2 = Guid.Parse("77777777-7777-7777-7777-777777777002");
    public static readonly Guid Product3 = Guid.Parse("77777777-7777-7777-7777-777777777003");
    public static readonly Guid Product4 = Guid.Parse("77777777-7777-7777-7777-777777777004");
    public static readonly Guid Product5 = Guid.Parse("77777777-7777-7777-7777-777777777005");

    /// <summary>
    /// Seed all pharmacy data (requires LookupSeeder to run first)
    /// </summary>
    public static async Task SeedAsync(PharmacyDbContext context)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await SeedRolesAsync(context);
                await SeedBranchesAsync(context);
                await SeedStakeholdersAsync(context);
                await SeedUsersAsync(context);
                await SeedProductsAsync(context);
                await SeedStockAsync(context);
                await SeedStockTransactionsAsync(context);
                await SeedSalesInvoicesAsync(context);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    private static async Task SeedRolesAsync(PharmacyDbContext context)
    {
        if (await context.Roles.AnyAsync()) return;

        var roles = new List<Role>
        {
            new() { Oid = RoleAdmin, Name = "Admin", Description = "System Administrator with full access" },
            new() { Oid = RoleManager, Name = "Manager", Description = "Branch Manager with management access" },
            new() { Oid = RoleCashier, Name = "Cashier", Description = "POS Cashier with sales access" },
            new() { Oid = RolePharmacist, Name = "Pharmacist", Description = "Licensed Pharmacist with dispensing access" }
        };

        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBranchesAsync(PharmacyDbContext context)
    {
        if (await context.Branches.AnyAsync()) return;

        var branches = new List<Branch>
        {
            new()
            {
                Oid = Branch1,
                BranchCode = "HQ-001",
                BranchName = "Health Center SOLELT GOHAENAH - Main Branch",
                GLN = "6820034800059",
                City = "Sana'a",
                District = "Al-Tahrir",
                Address = "Al-Tahrir Street, Building 15"
            },
            new()
            {
                Oid = Branch2,
                BranchCode = "BR-002",
                BranchName = "Health Center SOLELT GOHAENAH - Branch 2",
                GLN = "6820034800066",
                City = "Sana'a",
                District = "Al-Zubairi",
                Address = "Al-Zubairi Street, Building 22"
            }
        };

        context.Branches.AddRange(branches);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStakeholdersAsync(PharmacyDbContext context)
    {
        if (await context.Stakeholders.AnyAsync()) return;

        var stakeholders = new List<Stakeholder>
        {
            new()
            {
                Oid = PharmacyStakeholder,
                GLN = "6820034800059",
                Name = "Health Center SOLELT GOHAENAH",
                StakeholderTypeId = LookupSeeder.LookupDetailPharmacy,
                City = "Sana'a",
                District = "Al-Tahrir",
                Address = "Al-Tahrir Street",
                IsActive = true,
                ContactPerson = "Dr. Ahmed Ali",
                Phone = "+967771234567",
                Email = "info@healthcenter.ye"
            },
            new()
            {
                Oid = Supplier1,
                GLN = "1234567890123",
                Name = "Global Pharma Supplier",
                StakeholderTypeId = LookupSeeder.LookupDetailSupplier,
                City = "Dubai",
                District = "Healthcare City",
                Address = "Building A1, Healthcare City",
                IsActive = true,
                ContactPerson = "John Smith",
                Phone = "+971501234567",
                Email = "orders@globalpharma.com"
            },
            new()
            {
                Oid = Supplier2,
                GLN = "9876543210987",
                Name = "MediSupply International",
                StakeholderTypeId = LookupSeeder.LookupDetailSupplier,
                City = "Amman",
                District = "Abdali",
                Address = "Abdali Medical Complex",
                IsActive = true,
                ContactPerson = "Sara Hassan",
                Phone = "+962791234567",
                Email = "sales@medisupply.com"
            }
        };

        context.Stakeholders.AddRange(stakeholders);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(PharmacyDbContext context)
    {
        if (await context.SystemUsers.AnyAsync()) return;

        var (hash1, salt1) = HashPassword("Admin@123");
        var (hash2, salt2) = HashPassword("Manager@123");
        var (hash3, salt3) = HashPassword("Cashier@123");

        var users = new List<SystemUser>
        {
            // Branch 1 Users
            new()
            {
                Oid = AdminUser1,
                Username = "admin1",
                PasswordHash = hash1,
                PasswordSalt = salt1,
                Email = "admin1@healthcenter.ye",
                Mobile = "+967771111111",
                FirstName = "Ahmed",
                LastName = "Ali",
                FullName = "Ahmed Ali",
                GenderLookupId = LookupSeeder.LookupDetailMale,
                RoleId = RoleAdmin,
                BranchId = Branch1,
                IsActive = true
            },
            new()
            {
                Oid = ManagerUser1,
                Username = "manager1",
                PasswordHash = hash2,
                PasswordSalt = salt2,
                Email = "manager1@healthcenter.ye",
                Mobile = "+967772222222",
                FirstName = "Mohammed",
                LastName = "Hassan",
                FullName = "Mohammed Hassan",
                GenderLookupId = LookupSeeder.LookupDetailMale,
                RoleId = RoleManager,
                BranchId = Branch1,
                IsActive = true
            },
            new()
            {
                Oid = CashierUser1,
                Username = "cashier1",
                PasswordHash = hash3,
                PasswordSalt = salt3,
                Email = "cashier1@healthcenter.ye",
                Mobile = "+967773333333",
                FirstName = "Fatima",
                LastName = "Omar",
                FullName = "Fatima Omar",
                GenderLookupId = LookupSeeder.LookupDetailFemale,
                RoleId = RoleCashier,
                BranchId = Branch1,
                IsActive = true
            },
            // Branch 2 Users
            new()
            {
                Oid = AdminUser2,
                Username = "admin2",
                PasswordHash = hash1,
                PasswordSalt = salt1,
                Email = "admin2@healthcenter.ye",
                Mobile = "+967774444444",
                FirstName = "Khalid",
                LastName = "Saleh",
                FullName = "Khalid Saleh",
                GenderLookupId = LookupSeeder.LookupDetailMale,
                RoleId = RoleAdmin,
                BranchId = Branch2,
                IsActive = true
            },
            new()
            {
                Oid = ManagerUser2,
                Username = "manager2",
                PasswordHash = hash2,
                PasswordSalt = salt2,
                Email = "manager2@healthcenter.ye",
                Mobile = "+967775555555",
                FirstName = "Noor",
                LastName = "Ibrahim",
                FullName = "Noor Ibrahim",
                GenderLookupId = LookupSeeder.LookupDetailFemale,
                RoleId = RoleManager,
                BranchId = Branch2,
                IsActive = true
            },
            new()
            {
                Oid = CashierUser2,
                Username = "cashier2",
                PasswordHash = hash3,
                PasswordSalt = salt3,
                Email = "cashier2@healthcenter.ye",
                Mobile = "+967776666666",
                FirstName = "Ali",
                LastName = "Mahmoud",
                FullName = "Ali Mahmoud",
                GenderLookupId = LookupSeeder.LookupDetailMale,
                RoleId = RoleCashier,
                BranchId = Branch2,
                IsActive = true
            }
        };

        context.SystemUsers.AddRange(users);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(PharmacyDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        var products = new List<Product>
        {
            new()
            {
                Oid = Product1,
                GTIN = "06820034800001",
                DrugName = "Paracetamol 500mg Tablets",
                GenericName = "Paracetamol",
                ProductTypeId = LookupSeeder.LookupDetailTablet,
                StrengthValue = "500",
                StrengthUnit = "mg",
                PackageType = "Box",
                PackageSize = "30 tablets",
                Price = 5.00m,
                RegistrationNumber = "REG-001-2024",
                IsExportable = false,
                IsImportable = true,
                DrugStatus = "Active",
                MarketingStatus = "Marketed",
                LegalStatus = "OTC",
                Manufacturer = "PharmaCorp International",
                CountryOfOrigin = "India",
                MinStockLevel = 100,
                MaxStockLevel = 1000
            },
            new()
            {
                Oid = Product2,
                GTIN = "06820034800002",
                DrugName = "Amoxicillin 250mg/5ml Syrup",
                GenericName = "Amoxicillin",
                ProductTypeId = LookupSeeder.LookupDetailSyrup,
                StrengthValue = "250",
                StrengthUnit = "mg/5ml",
                PackageType = "Bottle",
                PackageSize = "100ml",
                Price = 12.50m,
                RegistrationNumber = "REG-002-2024",
                Volume = 100,
                UnitOfVolume = "ml",
                IsExportable = false,
                IsImportable = true,
                DrugStatus = "Active",
                MarketingStatus = "Marketed",
                LegalStatus = "Prescription",
                Manufacturer = "MediPharm Labs",
                CountryOfOrigin = "Jordan",
                MinStockLevel = 50,
                MaxStockLevel = 500
            },
            new()
            {
                Oid = Product3,
                GTIN = "06820034800003",
                DrugName = "Insulin Glargine 100IU/ml Injection",
                GenericName = "Insulin Glargine",
                ProductTypeId = LookupSeeder.LookupDetailInjection,
                StrengthValue = "100",
                StrengthUnit = "IU/ml",
                PackageType = "Vial",
                PackageSize = "10ml vial",
                Price = 85.00m,
                RegistrationNumber = "REG-003-2024",
                Volume = 10,
                UnitOfVolume = "ml",
                IsExportable = false,
                IsImportable = true,
                DrugStatus = "Active",
                MarketingStatus = "Marketed",
                LegalStatus = "Prescription",
                Manufacturer = "Sanofi",
                CountryOfOrigin = "France",
                MinStockLevel = 20,
                MaxStockLevel = 200
            },
            new()
            {
                Oid = Product4,
                GTIN = "06820034800004",
                DrugName = "Omeprazole 20mg Capsules",
                GenericName = "Omeprazole",
                ProductTypeId = LookupSeeder.LookupDetailCapsule,
                StrengthValue = "20",
                StrengthUnit = "mg",
                PackageType = "Box",
                PackageSize = "14 capsules",
                Price = 8.00m,
                RegistrationNumber = "REG-004-2024",
                IsExportable = false,
                IsImportable = true,
                DrugStatus = "Active",
                MarketingStatus = "Marketed",
                LegalStatus = "OTC",
                Manufacturer = "AstraZeneca",
                CountryOfOrigin = "UK",
                MinStockLevel = 80,
                MaxStockLevel = 800
            },
            new()
            {
                Oid = Product5,
                GTIN = "06820034800005",
                DrugName = "Betamethasone 0.1% Ointment",
                GenericName = "Betamethasone",
                ProductTypeId = LookupSeeder.LookupDetailOintment,
                StrengthValue = "0.1",
                StrengthUnit = "%",
                PackageType = "Tube",
                PackageSize = "30g",
                Price = 15.00m,
                RegistrationNumber = "REG-005-2024",
                Volume = 30,
                UnitOfVolume = "g",
                IsExportable = false,
                IsImportable = true,
                DrugStatus = "Active",
                MarketingStatus = "Marketed",
                LegalStatus = "Prescription",
                Manufacturer = "GSK",
                CountryOfOrigin = "Belgium",
                MinStockLevel = 40,
                MaxStockLevel = 400
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStockAsync(PharmacyDbContext context)
    {
        if (await context.Stocks.AnyAsync()) return;

        var stocks = new List<Stock>
        {
            // Branch 1 Stock
            new() { ProductId = Product1, BranchId = Branch1, Quantity = 500, ReservedQuantity = 0, AverageCost = 3.50m },
            new() { ProductId = Product2, BranchId = Branch1, Quantity = 200, ReservedQuantity = 0, AverageCost = 8.00m },
            new() { ProductId = Product3, BranchId = Branch1, Quantity = 50, ReservedQuantity = 0, AverageCost = 60.00m },
            new() { ProductId = Product4, BranchId = Branch1, Quantity = 300, ReservedQuantity = 0, AverageCost = 5.00m },
            new() { ProductId = Product5, BranchId = Branch1, Quantity = 100, ReservedQuantity = 0, AverageCost = 10.00m },
            
            // Branch 2 Stock
            new() { ProductId = Product1, BranchId = Branch2, Quantity = 400, ReservedQuantity = 0, AverageCost = 3.50m },
            new() { ProductId = Product2, BranchId = Branch2, Quantity = 150, ReservedQuantity = 0, AverageCost = 8.00m },
            new() { ProductId = Product3, BranchId = Branch2, Quantity = 30, ReservedQuantity = 0, AverageCost = 60.00m },
            new() { ProductId = Product4, BranchId = Branch2, Quantity = 250, ReservedQuantity = 0, AverageCost = 5.00m },
            new() { ProductId = Product5, BranchId = Branch2, Quantity = 80, ReservedQuantity = 0, AverageCost = 10.00m }
        };

        context.Stocks.AddRange(stocks);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStockTransactionsAsync(PharmacyDbContext context)
    {
        if (await context.StockTransactions.AnyAsync()) return;

        var today = DateTime.UtcNow;
        var transactions = new List<StockTransaction>
        {
            // Stock IN transactions from Supplier 1 to Branch 1
            new()
            {
                ProductId = Product1,
                ToBranchId = Branch1,
                Quantity = 500,
                TransactionTypeId = LookupSeeder.LookupDetailTransactionIn,
                ReferenceNumber = "STK-IN-20240101-0001",
                TransactionDate = today.AddDays(-30),
                UnitCost = 3.50m,
                TotalValue = 1750m,
                BatchNumber = "BATCH-001",
                ExpiryDate = today.AddYears(2),
                SupplierId = Supplier1,
                Notes = "Initial stock from Global Pharma Supplier"
            },
            new()
            {
                ProductId = Product2,
                ToBranchId = Branch1,
                Quantity = 200,
                TransactionTypeId = LookupSeeder.LookupDetailTransactionIn,
                ReferenceNumber = "STK-IN-20240101-0002",
                TransactionDate = today.AddDays(-30),
                UnitCost = 8.00m,
                TotalValue = 1600m,
                BatchNumber = "BATCH-002",
                ExpiryDate = today.AddYears(1),
                SupplierId = Supplier1,
                Notes = "Initial stock from Global Pharma Supplier"
            },
            // Stock IN transactions from Supplier 2 to Branch 2
            new()
            {
                ProductId = Product1,
                ToBranchId = Branch2,
                Quantity = 400,
                TransactionTypeId = LookupSeeder.LookupDetailTransactionIn,
                ReferenceNumber = "STK-IN-20240102-0001",
                TransactionDate = today.AddDays(-25),
                UnitCost = 3.50m,
                TotalValue = 1400m,
                BatchNumber = "BATCH-003",
                ExpiryDate = today.AddYears(2),
                SupplierId = Supplier2,
                Notes = "Stock from MediSupply International"
            },
            // Transfer from Branch 1 to Branch 2
            new()
            {
                ProductId = Product3,
                FromBranchId = Branch1,
                ToBranchId = Branch2,
                Quantity = 10,
                TransactionTypeId = LookupSeeder.LookupDetailTransactionTransfer,
                ReferenceNumber = "TRF-20240115-0001",
                TransactionDate = today.AddDays(-15),
                Notes = "Inter-branch transfer for stock balancing"
            }
        };

        context.StockTransactions.AddRange(transactions);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSalesInvoicesAsync(PharmacyDbContext context)
    {
        if (await context.SalesInvoices.AnyAsync()) return;

        var today = DateTime.UtcNow;

        // Branch 1 - Invoice 1
        var invoice1 = new SalesInvoice
        {
            InvoiceNumber = "INV-HQ-001-20240120-0001",
            BranchId = Branch1,
            CustomerName = "Mohammed Abdullah",
            CustomerPhone = "+967771234000",
            SubTotal = 25.50m,
            DiscountPercent = 0,
            DiscountAmount = 0,
            TaxAmount = 0,
            TotalAmount = 25.50m,
            PaidAmount = 30m,
            ChangeAmount = 4.50m,
            InvoiceDate = today.AddDays(-5),
            PaymentMethodId = LookupSeeder.LookupDetailPaymentCash,
            InvoiceStatusId = LookupSeeder.LookupDetailStatusCompleted,
            CashierId = CashierUser1
        };
        context.SalesInvoices.Add(invoice1);
        await context.SaveChangesAsync();

        var items1 = new List<SalesInvoiceItem>
        {
            new() { InvoiceId = invoice1.Oid, ProductId = Product1, Quantity = 2, UnitPrice = 5.00m, TotalPrice = 10.00m },
            new() { InvoiceId = invoice1.Oid, ProductId = Product4, Quantity = 1, UnitPrice = 8.00m, TotalPrice = 8.00m },
            new() { InvoiceId = invoice1.Oid, ProductId = Product5, Quantity = 0.5m, UnitPrice = 15.00m, TotalPrice = 7.50m }
        };
        context.SalesInvoiceItems.AddRange(items1);

        // Branch 1 - Invoice 2
        var invoice2 = new SalesInvoice
        {
            InvoiceNumber = "INV-HQ-001-20240120-0002",
            BranchId = Branch1,
            CustomerName = "Sara Ahmed",
            CustomerPhone = "+967772345000",
            SubTotal = 97.50m,
            DiscountPercent = 5,
            DiscountAmount = 4.875m,
            TaxAmount = 0,
            TotalAmount = 92.625m,
            PaidAmount = 100m,
            ChangeAmount = 7.375m,
            InvoiceDate = today.AddDays(-3),
            PaymentMethodId = LookupSeeder.LookupDetailPaymentCard,
            InvoiceStatusId = LookupSeeder.LookupDetailStatusCompleted,
            CashierId = CashierUser1,
            PrescriptionNumber = "RX-2024-0001",
            DoctorName = "Dr. Fatima Hassan"
        };
        context.SalesInvoices.Add(invoice2);
        await context.SaveChangesAsync();

        var items2 = new List<SalesInvoiceItem>
        {
            new() { InvoiceId = invoice2.Oid, ProductId = Product2, Quantity = 1, UnitPrice = 12.50m, TotalPrice = 12.50m },
            new() { InvoiceId = invoice2.Oid, ProductId = Product3, Quantity = 1, UnitPrice = 85.00m, TotalPrice = 85.00m }
        };
        context.SalesInvoiceItems.AddRange(items2);

        // Branch 2 - Invoice 1
        var invoice3 = new SalesInvoice
        {
            InvoiceNumber = "INV-BR-002-20240121-0001",
            BranchId = Branch2,
            CustomerName = "Ali Hassan",
            CustomerPhone = "+967773456000",
            SubTotal = 38.00m,
            TotalAmount = 38.00m,
            PaidAmount = 40m,
            ChangeAmount = 2.00m,
            InvoiceDate = today.AddDays(-2),
            PaymentMethodId = LookupSeeder.LookupDetailPaymentCash,
            InvoiceStatusId = LookupSeeder.LookupDetailStatusCompleted,
            CashierId = CashierUser2
        };
        context.SalesInvoices.Add(invoice3);
        await context.SaveChangesAsync();

        var items3 = new List<SalesInvoiceItem>
        {
            new() { InvoiceId = invoice3.Oid, ProductId = Product1, Quantity = 3, UnitPrice = 5.00m, TotalPrice = 15.00m },
            new() { InvoiceId = invoice3.Oid, ProductId = Product4, Quantity = 2, UnitPrice = 8.00m, TotalPrice = 16.00m },
            new() { InvoiceId = invoice3.Oid, ProductId = Product5, Quantity = 0.5m, UnitPrice = 14.00m, TotalPrice = 7.00m }
        };
        context.SalesInvoiceItems.AddRange(items3);

        // Branch 2 - Invoice 2 (with prescription)
        var invoice4 = new SalesInvoice
        {
            InvoiceNumber = "INV-BR-002-20240121-0002",
            BranchId = Branch2,
            CustomerName = "Nadia Khalil",
            CustomerPhone = "+967774567000",
            SubTotal = 127.50m,
            DiscountPercent = 10,
            DiscountAmount = 12.75m,
            TotalAmount = 114.75m,
            PaidAmount = 120m,
            ChangeAmount = 5.25m,
            InvoiceDate = today.AddDays(-1),
            PaymentMethodId = LookupSeeder.LookupDetailPaymentCash,
            InvoiceStatusId = LookupSeeder.LookupDetailStatusCompleted,
            CashierId = CashierUser2,
            PrescriptionNumber = "RX-2024-0002",
            DoctorName = "Dr. Ahmed Saleh"
        };
        context.SalesInvoices.Add(invoice4);
        await context.SaveChangesAsync();

        var items4 = new List<SalesInvoiceItem>
        {
            new() { InvoiceId = invoice4.Oid, ProductId = Product2, Quantity = 2, UnitPrice = 12.50m, TotalPrice = 25.00m },
            new() { InvoiceId = invoice4.Oid, ProductId = Product3, Quantity = 1, UnitPrice = 85.00m, TotalPrice = 85.00m },
            new() { InvoiceId = invoice4.Oid, ProductId = Product5, Quantity = 1, UnitPrice = 17.50m, TotalPrice = 17.50m }
        };
        context.SalesInvoiceItems.AddRange(items4);

        await context.SaveChangesAsync();
    }

    private static (string hash, string salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[32];
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));

        return (hash, salt);
    }
}
