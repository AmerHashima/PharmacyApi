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
            .ForMember(d => d.AccountNameAr, o => o.MapFrom(s => s.Account != null ? s.Account.AccountNameAr : null));
        CreateMap<CreateCashBoxDto, CashBox>();
        CreateMap<UpdateCashBoxDto, CashBox>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // BankAccount
        CreateMap<BankAccount, BankAccountDto>()
            .ForMember(d => d.BranchName,       o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.AccountNameAr,    o => o.MapFrom(s => s.Account != null ? s.Account.AccountNameAr : null))
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
    }
}
