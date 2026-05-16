namespace Pharmacy.Application.DTOs.Accounting;

public class AccountingSettingsDto
{
    public Guid Oid { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }

    public Guid? SalesAccountId { get; set; }
    public string? SalesAccountName { get; set; }

    public Guid? VatAccountId { get; set; }
    public string? VatAccountName { get; set; }

    public Guid? DiscountAccountId { get; set; }
    public string? DiscountAccountName { get; set; }

    public Guid? CogsAccountId { get; set; }
    public string? CogsAccountName { get; set; }

    public Guid? InventoryAccountId { get; set; }
    public string? InventoryAccountName { get; set; }

    public Guid? CashAccountId { get; set; }
    public string? CashAccountName { get; set; }

    public Guid? BankAccountId { get; set; }
    public string? BankAccountName { get; set; }

    public Guid? ReceivableAccountId { get; set; }
    public string? ReceivableAccountName { get; set; }

    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateAccountingSettingsDto
{
    public Guid BranchId { get; set; }
    public Guid? SalesAccountId { get; set; }
    public Guid? VatAccountId { get; set; }
    public Guid? DiscountAccountId { get; set; }
    public Guid? CogsAccountId { get; set; }
    public Guid? InventoryAccountId { get; set; }
    public Guid? CashAccountId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? ReceivableAccountId { get; set; }
}

public class UpdateAccountingSettingsDto
{
    public Guid Oid { get; set; }
    public Guid BranchId { get; set; }
    public Guid? SalesAccountId { get; set; }
    public Guid? VatAccountId { get; set; }
    public Guid? DiscountAccountId { get; set; }
    public Guid? CogsAccountId { get; set; }
    public Guid? InventoryAccountId { get; set; }
    public Guid? CashAccountId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? ReceivableAccountId { get; set; }
}
