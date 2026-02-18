using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Data;

/// <summary>
/// Seeds all AppLookup Master and Detail data for the pharmacy system.
/// This includes all foreign key references used throughout the solution.
/// </summary>
public static class LookupSeeder
{
    // ========================================
    // Static GUIDs for consistent seeding
    // ========================================

    #region Lookup Master IDs
    public static readonly Guid LookupMasterGender = Guid.Parse("11111111-1111-1111-1111-111111111001");
    public static readonly Guid LookupMasterMaritalStatus = Guid.Parse("11111111-1111-1111-1111-111111111002");
    public static readonly Guid LookupMasterStakeholderType = Guid.Parse("11111111-1111-1111-1111-111111111003");
    public static readonly Guid LookupMasterProductType = Guid.Parse("11111111-1111-1111-1111-111111111004");
    public static readonly Guid LookupMasterTransactionType = Guid.Parse("11111111-1111-1111-1111-111111111005");
    public static readonly Guid LookupMasterPaymentMethod = Guid.Parse("11111111-1111-1111-1111-111111111006");
    public static readonly Guid LookupMasterInvoiceStatus = Guid.Parse("11111111-1111-1111-1111-111111111007");
    public static readonly Guid LookupMasterBatchStatus = Guid.Parse("11111111-1111-1111-1111-111111111008");
    public static readonly Guid LookupMasterDrugStatus = Guid.Parse("11111111-1111-1111-1111-111111111009");
    public static readonly Guid LookupMasterLegalStatus = Guid.Parse("11111111-1111-1111-1111-111111111010");
    public static readonly Guid LookupMasterPackageType = Guid.Parse("11111111-1111-1111-1111-111111111011");
    public static readonly Guid LookupMasterStrengthUnit = Guid.Parse("11111111-1111-1111-1111-111111111012");
    #endregion

    #region Lookup Detail IDs - Gender
    public static readonly Guid LookupDetailMale = Guid.Parse("22222222-2222-2222-2222-222222222001");
    public static readonly Guid LookupDetailFemale = Guid.Parse("22222222-2222-2222-2222-222222222002");
    #endregion

    #region Lookup Detail IDs - Marital Status
    public static readonly Guid LookupDetailSingle = Guid.Parse("22222222-2222-2222-2222-222222222003");
    public static readonly Guid LookupDetailMarried = Guid.Parse("22222222-2222-2222-2222-222222222004");
    public static readonly Guid LookupDetailDivorced = Guid.Parse("22222222-2222-2222-2222-222222222005");
    public static readonly Guid LookupDetailWidowed = Guid.Parse("22222222-2222-2222-2222-222222222006");
    #endregion

    #region Lookup Detail IDs - Stakeholder Type
    public static readonly Guid LookupDetailPharmacy = Guid.Parse("22222222-2222-2222-2222-222222222010");
    public static readonly Guid LookupDetailSupplier = Guid.Parse("22222222-2222-2222-2222-222222222011");
    public static readonly Guid LookupDetailDistributor = Guid.Parse("22222222-2222-2222-2222-222222222012");
    public static readonly Guid LookupDetailManufacturer = Guid.Parse("22222222-2222-2222-2222-222222222013");
    public static readonly Guid LookupDetailWholesaler = Guid.Parse("22222222-2222-2222-2222-222222222014");
    #endregion

    #region Lookup Detail IDs - Product Type
    public static readonly Guid LookupDetailTablet = Guid.Parse("22222222-2222-2222-2222-222222222020");
    public static readonly Guid LookupDetailSyrup = Guid.Parse("22222222-2222-2222-2222-222222222021");
    public static readonly Guid LookupDetailInjection = Guid.Parse("22222222-2222-2222-2222-222222222022");
    public static readonly Guid LookupDetailCapsule = Guid.Parse("22222222-2222-2222-2222-222222222023");
    public static readonly Guid LookupDetailOintment = Guid.Parse("22222222-2222-2222-2222-222222222024");
    public static readonly Guid LookupDetailCream = Guid.Parse("22222222-2222-2222-2222-222222222025");
    public static readonly Guid LookupDetailDrops = Guid.Parse("22222222-2222-2222-2222-222222222026");
    public static readonly Guid LookupDetailSuppository = Guid.Parse("22222222-2222-2222-2222-222222222027");
    public static readonly Guid LookupDetailPowder = Guid.Parse("22222222-2222-2222-2222-222222222028");
    public static readonly Guid LookupDetailInhaler = Guid.Parse("22222222-2222-2222-2222-222222222029");
    #endregion

    #region Lookup Detail IDs - Transaction Type
    public static readonly Guid LookupDetailTransactionIn = Guid.Parse("22222222-2222-2222-2222-222222222030");
    public static readonly Guid LookupDetailTransactionOut = Guid.Parse("22222222-2222-2222-2222-222222222031");
    public static readonly Guid LookupDetailTransactionTransfer = Guid.Parse("22222222-2222-2222-2222-222222222032");
    public static readonly Guid LookupDetailTransactionAdjustment = Guid.Parse("22222222-2222-2222-2222-222222222033");
    public static readonly Guid LookupDetailTransactionReturn = Guid.Parse("22222222-2222-2222-2222-222222222034");
    public static readonly Guid LookupDetailTransactionExpired = Guid.Parse("22222222-2222-2222-2222-222222222035");
    public static readonly Guid LookupDetailTransactionDamaged = Guid.Parse("22222222-2222-2222-2222-222222222036");
    #endregion

    #region Lookup Detail IDs - Payment Method
    public static readonly Guid LookupDetailPaymentCash = Guid.Parse("22222222-2222-2222-2222-222222222040");
    public static readonly Guid LookupDetailPaymentCard = Guid.Parse("22222222-2222-2222-2222-222222222041");
    public static readonly Guid LookupDetailPaymentCredit = Guid.Parse("22222222-2222-2222-2222-222222222042");
    public static readonly Guid LookupDetailPaymentInsurance = Guid.Parse("22222222-2222-2222-2222-222222222043");
    public static readonly Guid LookupDetailPaymentMixed = Guid.Parse("22222222-2222-2222-2222-222222222044");
    #endregion

    #region Lookup Detail IDs - Invoice Status
    public static readonly Guid LookupDetailStatusPending = Guid.Parse("22222222-2222-2222-2222-222222222050");
    public static readonly Guid LookupDetailStatusCompleted = Guid.Parse("22222222-2222-2222-2222-222222222051");
    public static readonly Guid LookupDetailStatusCancelled = Guid.Parse("22222222-2222-2222-2222-222222222052");
    public static readonly Guid LookupDetailStatusRefunded = Guid.Parse("22222222-2222-2222-2222-222222222053");
    public static readonly Guid LookupDetailStatusPartialRefund = Guid.Parse("22222222-2222-2222-2222-222222222054");
    public static readonly Guid LookupDetailStatusOnHold = Guid.Parse("22222222-2222-2222-2222-222222222055");
    #endregion

    #region Lookup Detail IDs - Batch Status
    public static readonly Guid LookupDetailBatchActive = Guid.Parse("22222222-2222-2222-2222-222222222060");
    public static readonly Guid LookupDetailBatchExpired = Guid.Parse("22222222-2222-2222-2222-222222222061");
    public static readonly Guid LookupDetailBatchQuarantine = Guid.Parse("22222222-2222-2222-2222-222222222062");
    public static readonly Guid LookupDetailBatchDepleted = Guid.Parse("22222222-2222-2222-2222-222222222063");
    public static readonly Guid LookupDetailBatchRecalled = Guid.Parse("22222222-2222-2222-2222-222222222064");
    #endregion

    #region Lookup Detail IDs - Drug Status
    public static readonly Guid LookupDetailDrugActive = Guid.Parse("22222222-2222-2222-2222-222222222070");
    public static readonly Guid LookupDetailDrugDiscontinued = Guid.Parse("22222222-2222-2222-2222-222222222071");
    public static readonly Guid LookupDetailDrugPending = Guid.Parse("22222222-2222-2222-2222-222222222072");
    public static readonly Guid LookupDetailDrugSuspended = Guid.Parse("22222222-2222-2222-2222-222222222073");
    #endregion

    #region Lookup Detail IDs - Legal Status
    public static readonly Guid LookupDetailLegalOTC = Guid.Parse("22222222-2222-2222-2222-222222222080");
    public static readonly Guid LookupDetailLegalPrescription = Guid.Parse("22222222-2222-2222-2222-222222222081");
    public static readonly Guid LookupDetailLegalControlled = Guid.Parse("22222222-2222-2222-2222-222222222082");
    public static readonly Guid LookupDetailLegalNarcotic = Guid.Parse("22222222-2222-2222-2222-222222222083");
    #endregion

    #region Lookup Detail IDs - Package Type
    public static readonly Guid LookupDetailPackageBox = Guid.Parse("22222222-2222-2222-2222-222222222090");
    public static readonly Guid LookupDetailPackageBottle = Guid.Parse("22222222-2222-2222-2222-222222222091");
    public static readonly Guid LookupDetailPackageBlister = Guid.Parse("22222222-2222-2222-2222-222222222092");
    public static readonly Guid LookupDetailPackageTube = Guid.Parse("22222222-2222-2222-2222-222222222093");
    public static readonly Guid LookupDetailPackageVial = Guid.Parse("22222222-2222-2222-2222-222222222094");
    public static readonly Guid LookupDetailPackageAmpoule = Guid.Parse("22222222-2222-2222-2222-222222222095");
    public static readonly Guid LookupDetailPackageSachet = Guid.Parse("22222222-2222-2222-2222-222222222096");
    #endregion

    #region Lookup Detail IDs - Strength Unit
    public static readonly Guid LookupDetailUnitMg = Guid.Parse("22222222-2222-2222-2222-2222222220A0");
    public static readonly Guid LookupDetailUnitG = Guid.Parse("22222222-2222-2222-2222-2222222220A1");
    public static readonly Guid LookupDetailUnitMl = Guid.Parse("22222222-2222-2222-2222-2222222220A2");
    public static readonly Guid LookupDetailUnitL = Guid.Parse("22222222-2222-2222-2222-2222222220A3");
    public static readonly Guid LookupDetailUnitIU = Guid.Parse("22222222-2222-2222-2222-2222222220A4");
    public static readonly Guid LookupDetailUnitPercent = Guid.Parse("22222222-2222-2222-2222-2222222220A5");
    public static readonly Guid LookupDetailUnitMcg = Guid.Parse("22222222-2222-2222-2222-2222222220A6");
    #endregion

    /// <summary>
    /// Seeds all lookup data for the pharmacy system
    /// </summary>
    public static async Task SeedLookupDataAsync(PharmacyDbContext context)
    {
        // Check if already seeded
        if (await context.AppLookupMasters.AnyAsync())
        {
            return;
        }

        var masters = new List<AppLookupMaster>();
        var details = new List<AppLookupDetail>();

        // ========================================
        // 1. GENDER - Used in SystemUser.GenderLookupId
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterGender,
            LookupCode = "GENDER",
            LookupNameAr = "الجنس",
            LookupNameEn = "Gender",
            Description = "Gender options for users",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailMale, MasterID = LookupMasterGender, ValueCode = "M", ValueNameAr = "ذكر", ValueNameEn = "Male", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailFemale, MasterID = LookupMasterGender, ValueCode = "F", ValueNameAr = "أنثى", ValueNameEn = "Female", SortOrder = 2, IsActive = true }
        });

        // ========================================
        // 2. MARITAL_STATUS - General use
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterMaritalStatus,
            LookupCode = "MARITAL_STATUS",
            LookupNameAr = "الحالة الاجتماعية",
            LookupNameEn = "Marital Status",
            Description = "Marital status options",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailSingle, MasterID = LookupMasterMaritalStatus, ValueCode = "SINGLE", ValueNameAr = "أعزب", ValueNameEn = "Single", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailMarried, MasterID = LookupMasterMaritalStatus, ValueCode = "MARRIED", ValueNameAr = "متزوج", ValueNameEn = "Married", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailDivorced, MasterID = LookupMasterMaritalStatus, ValueCode = "DIVORCED", ValueNameAr = "مطلق", ValueNameEn = "Divorced", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailWidowed, MasterID = LookupMasterMaritalStatus, ValueCode = "WIDOWED", ValueNameAr = "أرمل", ValueNameEn = "Widowed", SortOrder = 4, IsActive = true }
        });

        // ========================================
        // 3. STAKEHOLDER_TYPE - Used in Stakeholder.StakeholderTypeId
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterStakeholderType,
            LookupCode = "STAKEHOLDER_TYPE",
            LookupNameAr = "نوع الجهة",
            LookupNameEn = "Stakeholder Type",
            Description = "Types of external entities (Pharmacy, Supplier, Distributor, etc.)",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailPharmacy, MasterID = LookupMasterStakeholderType, ValueCode = "PHARMACY", ValueNameAr = "صيدلية", ValueNameEn = "Pharmacy", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailSupplier, MasterID = LookupMasterStakeholderType, ValueCode = "SUPPLIER", ValueNameAr = "مورد", ValueNameEn = "Supplier", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailDistributor, MasterID = LookupMasterStakeholderType, ValueCode = "DISTRIBUTOR", ValueNameAr = "موزع", ValueNameEn = "Distributor", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailManufacturer, MasterID = LookupMasterStakeholderType, ValueCode = "MANUFACTURER", ValueNameAr = "مصنع", ValueNameEn = "Manufacturer", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailWholesaler, MasterID = LookupMasterStakeholderType, ValueCode = "WHOLESALER", ValueNameAr = "تاجر جملة", ValueNameEn = "Wholesaler", SortOrder = 5, IsActive = true }
        });

        // ========================================
        // 4. PRODUCT_TYPE - Used in Product.ProductTypeId
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterProductType,
            LookupCode = "PRODUCT_TYPE",
            LookupNameAr = "نوع المنتج",
            LookupNameEn = "Product Type",
            Description = "Pharmaceutical product dosage forms",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailTablet, MasterID = LookupMasterProductType, ValueCode = "TABLET", ValueNameAr = "أقراص", ValueNameEn = "Tablet", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailCapsule, MasterID = LookupMasterProductType, ValueCode = "CAPSULE", ValueNameAr = "كبسولات", ValueNameEn = "Capsule", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailSyrup, MasterID = LookupMasterProductType, ValueCode = "SYRUP", ValueNameAr = "شراب", ValueNameEn = "Syrup", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailInjection, MasterID = LookupMasterProductType, ValueCode = "INJECTION", ValueNameAr = "حقن", ValueNameEn = "Injection", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailOintment, MasterID = LookupMasterProductType, ValueCode = "OINTMENT", ValueNameAr = "مرهم", ValueNameEn = "Ointment", SortOrder = 5, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailCream, MasterID = LookupMasterProductType, ValueCode = "CREAM", ValueNameAr = "كريم", ValueNameEn = "Cream", SortOrder = 6, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailDrops, MasterID = LookupMasterProductType, ValueCode = "DROPS", ValueNameAr = "قطرة", ValueNameEn = "Drops", SortOrder = 7, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailSuppository, MasterID = LookupMasterProductType, ValueCode = "SUPPOSITORY", ValueNameAr = "تحاميل", ValueNameEn = "Suppository", SortOrder = 8, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPowder, MasterID = LookupMasterProductType, ValueCode = "POWDER", ValueNameAr = "بودرة", ValueNameEn = "Powder", SortOrder = 9, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailInhaler, MasterID = LookupMasterProductType, ValueCode = "INHALER", ValueNameAr = "بخاخ", ValueNameEn = "Inhaler", SortOrder = 10, IsActive = true }
        });

        // ========================================
        // 5. TRANSACTION_TYPE - Used in StockTransaction.TransactionTypeId
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterTransactionType,
            LookupCode = "TRANSACTION_TYPE",
            LookupNameAr = "نوع المعاملة",
            LookupNameEn = "Transaction Type",
            Description = "Stock transaction types (IN, OUT, TRANSFER, etc.)",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailTransactionIn, MasterID = LookupMasterTransactionType, ValueCode = "IN", ValueNameAr = "وارد", ValueNameEn = "Stock In", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailTransactionOut, MasterID = LookupMasterTransactionType, ValueCode = "OUT", ValueNameAr = "صادر", ValueNameEn = "Stock Out", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailTransactionTransfer, MasterID = LookupMasterTransactionType, ValueCode = "TRANSFER", ValueNameAr = "تحويل", ValueNameEn = "Transfer", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailTransactionAdjustment, MasterID = LookupMasterTransactionType, ValueCode = "ADJUSTMENT", ValueNameAr = "تسوية", ValueNameEn = "Adjustment", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailTransactionReturn, MasterID = LookupMasterTransactionType, ValueCode = "RETURN", ValueNameAr = "مرتجع", ValueNameEn = "Return", SortOrder = 5, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailTransactionExpired, MasterID = LookupMasterTransactionType, ValueCode = "EXPIRED", ValueNameAr = "منتهي الصلاحية", ValueNameEn = "Expired", SortOrder = 6, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailTransactionDamaged, MasterID = LookupMasterTransactionType, ValueCode = "DAMAGED", ValueNameAr = "تالف", ValueNameEn = "Damaged", SortOrder = 7, IsActive = true }
        });

        // ========================================
        // 6. PAYMENT_METHOD - Used in SalesInvoice.PaymentMethodId
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterPaymentMethod,
            LookupCode = "PAYMENT_METHOD",
            LookupNameAr = "طريقة الدفع",
            LookupNameEn = "Payment Method",
            Description = "Payment methods for sales transactions",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailPaymentCash, MasterID = LookupMasterPaymentMethod, ValueCode = "CASH", ValueNameAr = "نقدي", ValueNameEn = "Cash", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPaymentCard, MasterID = LookupMasterPaymentMethod, ValueCode = "CARD", ValueNameAr = "بطاقة", ValueNameEn = "Card", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPaymentCredit, MasterID = LookupMasterPaymentMethod, ValueCode = "CREDIT", ValueNameAr = "آجل", ValueNameEn = "Credit", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPaymentInsurance, MasterID = LookupMasterPaymentMethod, ValueCode = "INSURANCE", ValueNameAr = "تأمين", ValueNameEn = "Insurance", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPaymentMixed, MasterID = LookupMasterPaymentMethod, ValueCode = "MIXED", ValueNameAr = "مختلط", ValueNameEn = "Mixed", SortOrder = 5, IsActive = true }
        });

        // ========================================
        // 7. INVOICE_STATUS - Used in SalesInvoice.InvoiceStatusId
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterInvoiceStatus,
            LookupCode = "INVOICE_STATUS",
            LookupNameAr = "حالة الفاتورة",
            LookupNameEn = "Invoice Status",
            Description = "Sales invoice status options",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailStatusPending, MasterID = LookupMasterInvoiceStatus, ValueCode = "PENDING", ValueNameAr = "معلق", ValueNameEn = "Pending", SortOrder = 1, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailStatusCompleted, MasterID = LookupMasterInvoiceStatus, ValueCode = "COMPLETED", ValueNameAr = "مكتمل", ValueNameEn = "Completed", SortOrder = 2, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailStatusCancelled, MasterID = LookupMasterInvoiceStatus, ValueCode = "CANCELLED", ValueNameAr = "ملغي", ValueNameEn = "Cancelled", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailStatusRefunded, MasterID = LookupMasterInvoiceStatus, ValueCode = "REFUNDED", ValueNameAr = "مسترد", ValueNameEn = "Refunded", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailStatusPartialRefund, MasterID = LookupMasterInvoiceStatus, ValueCode = "PARTIAL_REFUND", ValueNameAr = "استرداد جزئي", ValueNameEn = "Partial Refund", SortOrder = 5, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailStatusOnHold, MasterID = LookupMasterInvoiceStatus, ValueCode = "ON_HOLD", ValueNameAr = "معلق", ValueNameEn = "On Hold", SortOrder = 6, IsActive = true }
        });

        // ========================================
        // 8. BATCH_STATUS - Used in ProductBatch.BatchStatusId
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterBatchStatus,
            LookupCode = "BATCH_STATUS",
            LookupNameAr = "حالة الدفعة",
            LookupNameEn = "Batch Status",
            Description = "Product batch status options",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailBatchActive, MasterID = LookupMasterBatchStatus, ValueCode = "ACTIVE", ValueNameAr = "نشط", ValueNameEn = "Active", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailBatchExpired, MasterID = LookupMasterBatchStatus, ValueCode = "EXPIRED", ValueNameAr = "منتهي", ValueNameEn = "Expired", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailBatchQuarantine, MasterID = LookupMasterBatchStatus, ValueCode = "QUARANTINE", ValueNameAr = "حجر", ValueNameEn = "Quarantine", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailBatchDepleted, MasterID = LookupMasterBatchStatus, ValueCode = "DEPLETED", ValueNameAr = "مستنفد", ValueNameEn = "Depleted", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailBatchRecalled, MasterID = LookupMasterBatchStatus, ValueCode = "RECALLED", ValueNameAr = "مسحوب", ValueNameEn = "Recalled", SortOrder = 5, IsActive = true }
        });

        // ========================================
        // 9. DRUG_STATUS - For Product.DrugStatus reference
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterDrugStatus,
            LookupCode = "DRUG_STATUS",
            LookupNameAr = "حالة الدواء",
            LookupNameEn = "Drug Status",
            Description = "Drug registration/marketing status",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailDrugActive, MasterID = LookupMasterDrugStatus, ValueCode = "ACTIVE", ValueNameAr = "نشط", ValueNameEn = "Active", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailDrugDiscontinued, MasterID = LookupMasterDrugStatus, ValueCode = "DISCONTINUED", ValueNameAr = "متوقف", ValueNameEn = "Discontinued", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailDrugPending, MasterID = LookupMasterDrugStatus, ValueCode = "PENDING", ValueNameAr = "قيد المراجعة", ValueNameEn = "Pending", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailDrugSuspended, MasterID = LookupMasterDrugStatus, ValueCode = "SUSPENDED", ValueNameAr = "موقوف", ValueNameEn = "Suspended", SortOrder = 4, IsActive = true }
        });

        // ========================================
        // 10. LEGAL_STATUS - For Product.LegalStatus reference
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterLegalStatus,
            LookupCode = "LEGAL_STATUS",
            LookupNameAr = "التصنيف القانوني",
            LookupNameEn = "Legal Status",
            Description = "Drug legal classification (OTC, Prescription, Controlled)",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailLegalOTC, MasterID = LookupMasterLegalStatus, ValueCode = "OTC", ValueNameAr = "بدون وصفة", ValueNameEn = "Over The Counter", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailLegalPrescription, MasterID = LookupMasterLegalStatus, ValueCode = "PRESCRIPTION", ValueNameAr = "بوصفة طبية", ValueNameEn = "Prescription", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailLegalControlled, MasterID = LookupMasterLegalStatus, ValueCode = "CONTROLLED", ValueNameAr = "خاضع للرقابة", ValueNameEn = "Controlled", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailLegalNarcotic, MasterID = LookupMasterLegalStatus, ValueCode = "NARCOTIC", ValueNameAr = "مخدر", ValueNameEn = "Narcotic", SortOrder = 4, IsActive = true }
        });

        // ========================================
        // 11. PACKAGE_TYPE - For Product.PackageType reference
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterPackageType,
            LookupCode = "PACKAGE_TYPE",
            LookupNameAr = "نوع العبوة",
            LookupNameEn = "Package Type",
            Description = "Product packaging types",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailPackageBox, MasterID = LookupMasterPackageType, ValueCode = "BOX", ValueNameAr = "علبة", ValueNameEn = "Box", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPackageBottle, MasterID = LookupMasterPackageType, ValueCode = "BOTTLE", ValueNameAr = "زجاجة", ValueNameEn = "Bottle", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPackageBlister, MasterID = LookupMasterPackageType, ValueCode = "BLISTER", ValueNameAr = "شريط", ValueNameEn = "Blister", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPackageTube, MasterID = LookupMasterPackageType, ValueCode = "TUBE", ValueNameAr = "أنبوب", ValueNameEn = "Tube", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPackageVial, MasterID = LookupMasterPackageType, ValueCode = "VIAL", ValueNameAr = "قارورة", ValueNameEn = "Vial", SortOrder = 5, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPackageAmpoule, MasterID = LookupMasterPackageType, ValueCode = "AMPOULE", ValueNameAr = "أمبولة", ValueNameEn = "Ampoule", SortOrder = 6, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailPackageSachet, MasterID = LookupMasterPackageType, ValueCode = "SACHET", ValueNameAr = "كيس", ValueNameEn = "Sachet", SortOrder = 7, IsActive = true }
        });

        // ========================================
        // 12. STRENGTH_UNIT - For Product.StrengthUnit reference
        // ========================================
        masters.Add(new AppLookupMaster
        {
            Oid = LookupMasterStrengthUnit,
            LookupCode = "STRENGTH_UNIT",
            LookupNameAr = "وحدة التركيز",
            LookupNameEn = "Strength Unit",
            Description = "Drug strength measurement units",
            IsSystem = true
        });

        details.AddRange(new[]
        {
            new AppLookupDetail { Oid = LookupDetailUnitMg, MasterID = LookupMasterStrengthUnit, ValueCode = "MG", ValueNameAr = "مليغرام", ValueNameEn = "Milligram (mg)", SortOrder = 1, IsDefault = true, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailUnitG, MasterID = LookupMasterStrengthUnit, ValueCode = "G", ValueNameAr = "غرام", ValueNameEn = "Gram (g)", SortOrder = 2, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailUnitMl, MasterID = LookupMasterStrengthUnit, ValueCode = "ML", ValueNameAr = "مليلتر", ValueNameEn = "Milliliter (ml)", SortOrder = 3, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailUnitL, MasterID = LookupMasterStrengthUnit, ValueCode = "L", ValueNameAr = "لتر", ValueNameEn = "Liter (L)", SortOrder = 4, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailUnitIU, MasterID = LookupMasterStrengthUnit, ValueCode = "IU", ValueNameAr = "وحدة دولية", ValueNameEn = "International Unit (IU)", SortOrder = 5, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailUnitPercent, MasterID = LookupMasterStrengthUnit, ValueCode = "PERCENT", ValueNameAr = "نسبة مئوية", ValueNameEn = "Percent (%)", SortOrder = 6, IsActive = true },
            new AppLookupDetail { Oid = LookupDetailUnitMcg, MasterID = LookupMasterStrengthUnit, ValueCode = "MCG", ValueNameAr = "ميكروغرام", ValueNameEn = "Microgram (mcg)", SortOrder = 7, IsActive = true }
        });

        // ========================================
        // Save all data
        // ========================================
        context.AppLookupMasters.AddRange(masters);
        await context.SaveChangesAsync();

        context.AppLookupDetails.AddRange(details);
        await context.SaveChangesAsync();
    }
}