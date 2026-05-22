using AutoMapper;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Application.Mappings.Accounting;

public class AccountingProfile : Profile
{
    public AccountingProfile()
    {
        // FiscalYear
        CreateMap<FiscalYear, FiscalYearDto>();
        CreateMap<CreateFiscalYearDto, FiscalYear>();
        CreateMap<UpdateFiscalYearDto, FiscalYear>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // Account
        CreateMap<Account, AccountDto>()
            .ForMember(d => d.ParentNameAr,       o => o.MapFrom(s => s.Parent != null ? s.Parent.AccountNameAr : null))
            .ForMember(d => d.AccountTypeName,    o => o.MapFrom(s => s.AccountType != null ? s.AccountType.ValueNameEn : null))
            .ForMember(d => d.AccountTypeNameAr,  o => o.MapFrom(s => s.AccountType != null ? s.AccountType.ValueNameAr : null))
            .ForMember(d => d.NatureName,         o => o.MapFrom(s => s.Nature != null ? s.Nature.ValueNameEn : null))
            .ForMember(d => d.NatureNameAr,       o => o.MapFrom(s => s.Nature != null ? s.Nature.ValueNameAr : null))
            .ForMember(d => d.FinalAccountName,   o => o.MapFrom(s => s.FinalAccount != null ? s.FinalAccount.ValueNameEn : null))
            .ForMember(d => d.FinalAccountNameAr, o => o.MapFrom(s => s.FinalAccount != null ? s.FinalAccount.ValueNameAr : null));
        CreateMap<CreateAccountDto, Account>();
        CreateMap<UpdateAccountDto, Account>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // CostCenter
        CreateMap<CostCenter, CostCenterDto>()
            .ForMember(d => d.ParentNameAr, o => o.MapFrom(s => s.Parent != null ? s.Parent.NameAr : null));
        CreateMap<CreateCostCenterDto, CostCenter>();
        CreateMap<UpdateCostCenterDto, CostCenter>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // CashBox
        CreateMap<CashBox, CashBoxDto>()
            .ForMember(d => d.BranchName,    o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.AccountNameAr, o => o.MapFrom(s => s.ChildAccount != null ? s.ChildAccount.AccountNameAr : null));
        CreateMap<CreateCashBoxDto, CashBox>();
        CreateMap<UpdateCashBoxDto, CashBox>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // BankAccount
        CreateMap<BankAccount, BankAccountDto>()
            .ForMember(d => d.BranchName,       o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.AccountNameAr,    o => o.MapFrom(s => s.ChildAccount != null ? s.ChildAccount.AccountNameAr : null))
            .ForMember(d => d.CurrencyCodeName, o => o.MapFrom(s => s.CurrencyCode != null ? s.CurrencyCode.ValueNameEn : null));
        CreateMap<CreateBankAccountDto, BankAccount>();
        CreateMap<UpdateBankAccountDto, BankAccount>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // JournalEntryDetail → flat report DTO
        CreateMap<JournalEntryDetail, JournalEntryDetailReportDto>()
            .ForMember(d => d.AccountCode,          o => o.MapFrom(s => s.Account != null ? s.Account.AccountCode : null))
            .ForMember(d => d.AccountNameAr,        o => o.MapFrom(s => s.Account != null ? s.Account.AccountNameAr : null))
            .ForMember(d => d.AccountNameEn,        o => o.MapFrom(s => s.Account != null ? s.Account.AccountNameEn : null))
            .ForMember(d => d.CostCenterCode,       o => o.MapFrom(s => s.CostCenter != null ? s.CostCenter.Code : null))
            .ForMember(d => d.CostCenterNameAr,     o => o.MapFrom(s => s.CostCenter != null ? s.CostCenter.NameAr : null))
            .ForMember(d => d.EntryNumber,          o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryNumber : null))
            .ForMember(d => d.EntryDate,            o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryDate : default))
            .ForMember(d => d.EntryDescription,     o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.Description : null))
            .ForMember(d => d.TotalDebit,           o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.TotalDebit : 0))
            .ForMember(d => d.TotalCredit,          o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.TotalCredit : 0))
            .ForMember(d => d.IsPosted,             o => o.MapFrom(s => s.JournalEntry != null && s.JournalEntry.IsPosted))
            .ForMember(d => d.IsReversed,           o => o.MapFrom(s => s.JournalEntry != null && s.JournalEntry.IsReversed))
            .ForMember(d => d.ReferenceId,          o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.ReferenceId : null))
            .ForMember(d => d.FiscalYearId,         o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.FiscalYearId : null))
            .ForMember(d => d.FiscalYearNameAr,     o => o.MapFrom(s => s.JournalEntry != null && s.JournalEntry.FiscalYear != null ? s.JournalEntry.FiscalYear.NameAR : null))
            .ForMember(d => d.FiscalYearNameEn,     o => o.MapFrom(s => s.JournalEntry != null && s.JournalEntry.FiscalYear != null ? s.JournalEntry.FiscalYear.NameEn : null))
            .ForMember(d => d.BranchId,             o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.BranchId : null))
            .ForMember(d => d.BranchName,           o => o.MapFrom(s => s.JournalEntry != null && s.JournalEntry.Branch != null ? s.JournalEntry.Branch.BranchName : null))
            .ForMember(d => d.ReferenceTypeId,      o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.ReferenceTypeId : null))
            .ForMember(d => d.ReferenceTypeName,    o => o.MapFrom(s => s.JournalEntry != null && s.JournalEntry.ReferenceType != null ? s.JournalEntry.ReferenceType.ValueNameEn : null))
            .ForMember(d => d.ReferenceTypeNameAr,  o => o.MapFrom(s => s.JournalEntry != null && s.JournalEntry.ReferenceType != null ? s.JournalEntry.ReferenceType.ValueNameAr : null));

        // JournalEntryDetail
        CreateMap<JournalEntryDetail, JournalEntryDetailDto>()
            .ForMember(d => d.AccountCode,      o => o.MapFrom(s => s.Account != null ? s.Account.AccountCode : null))
            .ForMember(d => d.AccountNameAr,    o => o.MapFrom(s => s.Account != null ? s.Account.AccountNameAr : null))
            .ForMember(d => d.CostCenterNameAr, o => o.MapFrom(s => s.CostCenter != null ? s.CostCenter.NameAr : null));
        CreateMap<CreateJournalEntryDetailDto, JournalEntryDetail>();
        CreateMap<UpdateJournalEntryDetailDto, JournalEntryDetail>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // JournalEntry
        CreateMap<JournalEntry, JournalEntryDto>()
            .ForMember(d => d.FiscalYearName,    o => o.MapFrom(s => s.FiscalYear != null ? s.FiscalYear.NameEn : null))
            .ForMember(d => d.BranchName,        o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.ReferenceTypeName, o => o.MapFrom(s => s.ReferenceType != null ? s.ReferenceType.ValueNameEn : null));
        CreateMap<JournalEntry, JournalEntryMasterDto>()
            .ForMember(d => d.FiscalYearName, o => o.MapFrom(s => s.FiscalYear != null ? s.FiscalYear.NameEn : null))
            .ForMember(d => d.BranchName,     o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null));
        CreateMap<CreateJournalEntryDto, JournalEntry>()
            .ForMember(d => d.Details, o => o.Ignore());
        CreateMap<UpdateJournalEntryDto, JournalEntry>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore())
            .ForMember(d => d.Details,   o => o.Ignore());

        // ReceiptVoucherDetail
        CreateMap<ReceiptVoucherDetail, ReceiptVoucherDetailDto>()
            .ForMember(d => d.AccountCode,      o => o.MapFrom(s => s.Account != null ? s.Account.AccountCode : null))
            .ForMember(d => d.AccountNameAr,    o => o.MapFrom(s => s.Account != null ? s.Account.AccountNameAr : null))
            .ForMember(d => d.CostCenterNameAr, o => o.MapFrom(s => s.CostCenter != null ? s.CostCenter.NameAr : null))
            .ForMember(d => d.CustomerName,     o => o.MapFrom(s => s.Customer != null ? s.Customer.NameEN : null));
        CreateMap<CreateReceiptVoucherDetailDto, ReceiptVoucherDetail>();
        CreateMap<UpdateReceiptVoucherDetailDto, ReceiptVoucherDetail>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // ReceiptVoucher
        CreateMap<ReceiptVoucher, ReceiptVoucherDto>()
            .ForMember(d => d.FiscalYearName,     o => o.MapFrom(s => s.FiscalYear != null ? s.FiscalYear.NameEn : null))
            .ForMember(d => d.BranchName,         o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.CashBoxName,        o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameAr : null))
            .ForMember(d => d.BankAccountName,    o => o.MapFrom(s => s.BankAccount != null ? s.BankAccount.NameAr : null))
            .ForMember(d => d.JournalEntryNumber, o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryNumber : null));
        CreateMap<ReceiptVoucher, ReceiptVoucherMasterDto>()
            .ForMember(d => d.BranchName,         o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.CashBoxName,        o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameAr : null))
            .ForMember(d => d.BankAccountName,    o => o.MapFrom(s => s.BankAccount != null ? s.BankAccount.NameAr : null))
            .ForMember(d => d.JournalEntryNumber, o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryNumber : null));
        CreateMap<CreateReceiptVoucherDto, ReceiptVoucher>()
            .ForMember(d => d.Details, o => o.Ignore());
        CreateMap<UpdateReceiptVoucherDto, ReceiptVoucher>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore())
            .ForMember(d => d.Details,   o => o.Ignore());

        // PaymentVoucherDetail
        CreateMap<PaymentVoucherDetail, PaymentVoucherDetailDto>()
            .ForMember(d => d.AccountCode,      o => o.MapFrom(s => s.Account != null ? s.Account.AccountCode : null))
            .ForMember(d => d.AccountNameAr,    o => o.MapFrom(s => s.Account != null ? s.Account.AccountNameAr : null))
            .ForMember(d => d.CostCenterNameAr, o => o.MapFrom(s => s.CostCenter != null ? s.CostCenter.NameAr : null))
            .ForMember(d => d.StakeholderName,  o => o.MapFrom(s => s.Stakeholder != null ? s.Stakeholder.Name : null));
        CreateMap<CreatePaymentVoucherDetailDto, PaymentVoucherDetail>();
        CreateMap<UpdatePaymentVoucherDetailDto, PaymentVoucherDetail>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // PaymentVoucher
        CreateMap<PaymentVoucher, PaymentVoucherDto>()
            .ForMember(d => d.FiscalYearName,     o => o.MapFrom(s => s.FiscalYear != null ? s.FiscalYear.NameEn : null))
            .ForMember(d => d.BranchName,         o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.CashBoxName,        o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameAr : null))
            .ForMember(d => d.BankAccountName,    o => o.MapFrom(s => s.BankAccount != null ? s.BankAccount.NameAr : null))
            .ForMember(d => d.JournalEntryNumber, o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryNumber : null));
        CreateMap<PaymentVoucher, PaymentVoucherMasterDto>()
            .ForMember(d => d.BranchName,         o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.CashBoxName,        o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameAr : null))
            .ForMember(d => d.BankAccountName,    o => o.MapFrom(s => s.BankAccount != null ? s.BankAccount.NameAr : null))
            .ForMember(d => d.JournalEntryNumber, o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryNumber : null));
        CreateMap<CreatePaymentVoucherDto, PaymentVoucher>()
            .ForMember(d => d.Details, o => o.Ignore());
        CreateMap<UpdatePaymentVoucherDto, PaymentVoucher>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore())
            .ForMember(d => d.Details,   o => o.Ignore());
            // AccountingSettings
            CreateMap<AccountingSettings, AccountingSettingsDto>()
                .ForMember(d => d.BranchName,                        o => o.MapFrom(s => s.Branch != null                        ? s.Branch.BranchName                        : null))
                // Sales
                .ForMember(d => d.SalesAccountName,                  o => o.MapFrom(s => s.SalesAccount != null                  ? s.SalesAccount.AccountNameAr                  : null))
                .ForMember(d => d.SalesWithoutVatAccountName,        o => o.MapFrom(s => s.SalesWithoutVatAccount != null        ? s.SalesWithoutVatAccount.AccountNameAr        : null))
                .ForMember(d => d.SalesReturnAccountName,            o => o.MapFrom(s => s.SalesReturnAccount != null            ? s.SalesReturnAccount.AccountNameAr            : null))
                .ForMember(d => d.SalesDiscountAccountName,          o => o.MapFrom(s => s.SalesDiscountAccount != null          ? s.SalesDiscountAccount.AccountNameAr          : null))
                .ForMember(d => d.ZeroRatedSalesAccountName,         o => o.MapFrom(s => s.ZeroRatedSalesAccount != null         ? s.ZeroRatedSalesAccount.AccountNameAr         : null))
                .ForMember(d => d.ExemptSalesAccountName,            o => o.MapFrom(s => s.ExemptSalesAccount != null            ? s.ExemptSalesAccount.AccountNameAr            : null))
                .ForMember(d => d.DeferredRevenueAccountName,        o => o.MapFrom(s => s.DeferredRevenueAccount != null        ? s.DeferredRevenueAccount.AccountNameAr        : null))
                .ForMember(d => d.LoyaltyPointsAccountName,          o => o.MapFrom(s => s.LoyaltyPointsAccount != null          ? s.LoyaltyPointsAccount.AccountNameAr          : null))
                .ForMember(d => d.GiftCardAccountName,               o => o.MapFrom(s => s.GiftCardAccount != null               ? s.GiftCardAccount.AccountNameAr               : null))
                .ForMember(d => d.SalesCommissionAccountName,        o => o.MapFrom(s => s.SalesCommissionAccount != null        ? s.SalesCommissionAccount.AccountNameAr        : null))
                // VAT / Tax
                .ForMember(d => d.VatAccountName,                    o => o.MapFrom(s => s.VatAccount != null                    ? s.VatAccount.AccountNameAr                    : null))
                .ForMember(d => d.VatOutputAccountName,              o => o.MapFrom(s => s.VatOutputAccount != null              ? s.VatOutputAccount.AccountNameAr              : null))
                .ForMember(d => d.VatInputAccountName,               o => o.MapFrom(s => s.VatInputAccount != null               ? s.VatInputAccount.AccountNameAr               : null))
                .ForMember(d => d.VatSettlementAccountName,          o => o.MapFrom(s => s.VatSettlementAccount != null          ? s.VatSettlementAccount.AccountNameAr          : null))
                .ForMember(d => d.WithholdingTaxAccountName,         o => o.MapFrom(s => s.WithholdingTaxAccount != null         ? s.WithholdingTaxAccount.AccountNameAr         : null))
                .ForMember(d => d.VatSuspenseAccountName,            o => o.MapFrom(s => s.VatSuspenseAccount != null            ? s.VatSuspenseAccount.AccountNameAr            : null))
                // Purchases
                .ForMember(d => d.PurchaseAccountName,               o => o.MapFrom(s => s.PurchaseAccount != null               ? s.PurchaseAccount.AccountNameAr               : null))
                .ForMember(d => d.PurchaseWithoutVatAccountName,     o => o.MapFrom(s => s.PurchaseWithoutVatAccount != null     ? s.PurchaseWithoutVatAccount.AccountNameAr     : null))
                .ForMember(d => d.PurchaseVatAccountName,            o => o.MapFrom(s => s.PurchaseVatAccount != null            ? s.PurchaseVatAccount.AccountNameAr            : null))
                .ForMember(d => d.PurchaseReturnAccountName,         o => o.MapFrom(s => s.PurchaseReturnAccount != null         ? s.PurchaseReturnAccount.AccountNameAr         : null))
                .ForMember(d => d.PurchaseDiscountAccountName,       o => o.MapFrom(s => s.PurchaseDiscountAccount != null       ? s.PurchaseDiscountAccount.AccountNameAr       : null))
                .ForMember(d => d.PurchaseAccrualAccountName,        o => o.MapFrom(s => s.PurchaseAccrualAccount != null        ? s.PurchaseAccrualAccount.AccountNameAr        : null))
                .ForMember(d => d.FreightExpenseAccountName,         o => o.MapFrom(s => s.FreightExpenseAccount != null         ? s.FreightExpenseAccount.AccountNameAr         : null))
                // Inventory
                .ForMember(d => d.InventoryAccountName,              o => o.MapFrom(s => s.InventoryAccount != null              ? s.InventoryAccount.AccountNameAr              : null))
                .ForMember(d => d.InventoryAdjustmentAccountName,    o => o.MapFrom(s => s.InventoryAdjustmentAccount != null    ? s.InventoryAdjustmentAccount.AccountNameAr    : null))
                .ForMember(d => d.InventoryLossAccountName,          o => o.MapFrom(s => s.InventoryLossAccount != null          ? s.InventoryLossAccount.AccountNameAr          : null))
                .ForMember(d => d.InventoryGainAccountName,          o => o.MapFrom(s => s.InventoryGainAccount != null          ? s.InventoryGainAccount.AccountNameAr          : null))
                .ForMember(d => d.DamagedInventoryAccountName,       o => o.MapFrom(s => s.DamagedInventoryAccount != null       ? s.DamagedInventoryAccount.AccountNameAr       : null))
                .ForMember(d => d.ExpiredItemsAccountName,           o => o.MapFrom(s => s.ExpiredItemsAccount != null           ? s.ExpiredItemsAccount.AccountNameAr           : null))
                .ForMember(d => d.StockOpeningAccountName,           o => o.MapFrom(s => s.StockOpeningAccount != null           ? s.StockOpeningAccount.AccountNameAr           : null))
                .ForMember(d => d.StockClosingAccountName,           o => o.MapFrom(s => s.StockClosingAccount != null           ? s.StockClosingAccount.AccountNameAr           : null))
                .ForMember(d => d.StockTransferAccountName,          o => o.MapFrom(s => s.StockTransferAccount != null          ? s.StockTransferAccount.AccountNameAr          : null))
                // COGS
                .ForMember(d => d.CogsAccountName,                   o => o.MapFrom(s => s.CogsAccount != null                   ? s.CogsAccount.AccountNameAr                   : null))
                // Cash / POS / Bank
                .ForMember(d => d.CashAccountName,                   o => o.MapFrom(s => s.CashAccount != null                   ? s.CashAccount.AccountNameAr                   : null))
                .ForMember(d => d.PosAccountName,                    o => o.MapFrom(s => s.PosAccount != null                    ? s.PosAccount.AccountNameAr                    : null))
                .ForMember(d => d.PettyCashAccountName,              o => o.MapFrom(s => s.PettyCashAccount != null              ? s.PettyCashAccount.AccountNameAr              : null))
                .ForMember(d => d.CashDifferenceAccountName,         o => o.MapFrom(s => s.CashDifferenceAccount != null         ? s.CashDifferenceAccount.AccountNameAr         : null))
                .ForMember(d => d.BankAccountName,                   o => o.MapFrom(s => s.BankAccount != null                   ? s.BankAccount.AccountNameAr                   : null))
                .ForMember(d => d.BankFeesAccountName,               o => o.MapFrom(s => s.BankFeesAccount != null               ? s.BankFeesAccount.AccountNameAr               : null))
                .ForMember(d => d.ChequeAccountName,                 o => o.MapFrom(s => s.ChequeAccount != null                 ? s.ChequeAccount.AccountNameAr                 : null))
                // Customer / Supplier
                .ForMember(d => d.ReceivableAccountName,             o => o.MapFrom(s => s.ReceivableAccount != null             ? s.ReceivableAccount.AccountNameAr             : null))
                .ForMember(d => d.CustomerAdvanceAccountName,        o => o.MapFrom(s => s.CustomerAdvanceAccount != null        ? s.CustomerAdvanceAccount.AccountNameAr        : null))
                .ForMember(d => d.CustomerRefundAccountName,         o => o.MapFrom(s => s.CustomerRefundAccount != null         ? s.CustomerRefundAccount.AccountNameAr         : null))
                .ForMember(d => d.SupplierAdvanceAccountName,        o => o.MapFrom(s => s.SupplierAdvanceAccount != null        ? s.SupplierAdvanceAccount.AccountNameAr        : null))
                .ForMember(d => d.SupplierPayableAccountName,        o => o.MapFrom(s => s.SupplierPayableAccount != null        ? s.SupplierPayableAccount.AccountNameAr        : null))
                .ForMember(d => d.BadDebtAccountName,                o => o.MapFrom(s => s.BadDebtAccount != null                ? s.BadDebtAccount.AccountNameAr                : null))
                // Expenses
                .ForMember(d => d.GeneralExpenseAccountName,         o => o.MapFrom(s => s.GeneralExpenseAccount != null         ? s.GeneralExpenseAccount.AccountNameAr         : null))
                .ForMember(d => d.SalaryExpenseAccountName,          o => o.MapFrom(s => s.SalaryExpenseAccount != null          ? s.SalaryExpenseAccount.AccountNameAr          : null))
                .ForMember(d => d.RentExpenseAccountName,            o => o.MapFrom(s => s.RentExpenseAccount != null            ? s.RentExpenseAccount.AccountNameAr            : null))
                .ForMember(d => d.ElectricityExpenseAccountName,     o => o.MapFrom(s => s.ElectricityExpenseAccount != null     ? s.ElectricityExpenseAccount.AccountNameAr     : null))
                .ForMember(d => d.InternetExpenseAccountName,        o => o.MapFrom(s => s.InternetExpenseAccount != null        ? s.InternetExpenseAccount.AccountNameAr        : null))
                // System
                .ForMember(d => d.RoundOffAccountName,               o => o.MapFrom(s => s.RoundOffAccount != null               ? s.RoundOffAccount.AccountNameAr               : null))
                .ForMember(d => d.ExchangeRateDifferenceAccountName, o => o.MapFrom(s => s.ExchangeRateDifferenceAccount != null ? s.ExchangeRateDifferenceAccount.AccountNameAr : null))
                .ForMember(d => d.YearEndClosingAccountName,         o => o.MapFrom(s => s.YearEndClosingAccount != null         ? s.YearEndClosingAccount.AccountNameAr         : null));
            CreateMap<CreateAccountingSettingsDto, AccountingSettings>();
            CreateMap<UpdateAccountingSettingsDto, AccountingSettings>()
                .ForMember(d => d.Oid,       o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore());
        }
    }
