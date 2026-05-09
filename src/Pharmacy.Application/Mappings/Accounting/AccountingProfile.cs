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
            .ForMember(d => d.ParentNameAr,     o => o.MapFrom(s => s.Parent != null ? s.Parent.AccountNameAr : null))
            .ForMember(d => d.AccountTypeName,  o => o.MapFrom(s => s.AccountType != null ? s.AccountType.ValueNameEn : null))
            .ForMember(d => d.AccountTypeNameAr,o => o.MapFrom(s => s.AccountType != null ? s.AccountType.ValueNameAr : null))
            .ForMember(d => d.NatureName,       o => o.MapFrom(s => s.Nature != null ? s.Nature.ValueNameEn : null))
            .ForMember(d => d.NatureNameAr,     o => o.MapFrom(s => s.Nature != null ? s.Nature.ValueNameAr : null));
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
            .ForMember(d => d.FiscalYearName, o => o.MapFrom(s => s.FiscalYear != null ? s.FiscalYear.NameEn : null))
            .ForMember(d => d.BranchName,     o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null));
        CreateMap<CreateJournalEntryDto, JournalEntry>()
            .ForMember(d => d.Details, o => o.Ignore());
        CreateMap<UpdateJournalEntryDto, JournalEntry>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore())
            .ForMember(d => d.Details,   o => o.Ignore());

        // ReceiptVoucher
        CreateMap<ReceiptVoucher, ReceiptVoucherDto>()
            .ForMember(d => d.CustomerName,       o => o.MapFrom(s => s.Customer != null ? s.Customer.NameEN : null))
            .ForMember(d => d.CashBoxName,        o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameAr : null))
            .ForMember(d => d.BankAccountName,    o => o.MapFrom(s => s.BankAccount != null ? s.BankAccount.NameAr : null))
            .ForMember(d => d.JournalEntryNumber, o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryNumber : null));
        CreateMap<CreateReceiptVoucherDto, ReceiptVoucher>();
        CreateMap<UpdateReceiptVoucherDto, ReceiptVoucher>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());

        // PaymentVoucher
        CreateMap<PaymentVoucher, PaymentVoucherDto>()
            .ForMember(d => d.StakeholderName,    o => o.MapFrom(s => s.Stakeholder != null ? s.Stakeholder.Name : null))
            .ForMember(d => d.CashBoxName,        o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameAr : null))
            .ForMember(d => d.BankAccountName,    o => o.MapFrom(s => s.BankAccount != null ? s.BankAccount.NameAr : null))
            .ForMember(d => d.JournalEntryNumber, o => o.MapFrom(s => s.JournalEntry != null ? s.JournalEntry.EntryNumber : null));
        CreateMap<CreatePaymentVoucherDto, PaymentVoucher>();
        CreateMap<UpdatePaymentVoucherDto, PaymentVoucher>()
            .ForMember(d => d.Oid,       o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.CreatedBy, o => o.Ignore());
    }
}
