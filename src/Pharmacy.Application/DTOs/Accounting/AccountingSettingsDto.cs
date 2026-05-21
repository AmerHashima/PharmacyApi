namespace Pharmacy.Application.DTOs.Accounting;

public class AccountingSettingsDto
{
    public Guid Oid { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // SALES
    // ═════════════════════════════════════════════════════════════════════
    public Guid? SalesAccountId { get; set; }
    public string? SalesAccountName { get; set; }

    public Guid? SalesWithoutVatAccountId { get; set; }
    public string? SalesWithoutVatAccountName { get; set; }

    public Guid? SalesReturnAccountId { get; set; }
    public string? SalesReturnAccountName { get; set; }

    public Guid? SalesDiscountAccountId { get; set; }
    public string? SalesDiscountAccountName { get; set; }

    public Guid? ZeroRatedSalesAccountId { get; set; }
    public string? ZeroRatedSalesAccountName { get; set; }

    public Guid? ExemptSalesAccountId { get; set; }
    public string? ExemptSalesAccountName { get; set; }

    public Guid? DeferredRevenueAccountId { get; set; }
    public string? DeferredRevenueAccountName { get; set; }

    public Guid? LoyaltyPointsAccountId { get; set; }
    public string? LoyaltyPointsAccountName { get; set; }

    public Guid? GiftCardAccountId { get; set; }
    public string? GiftCardAccountName { get; set; }

    public Guid? SalesCommissionAccountId { get; set; }
    public string? SalesCommissionAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // VAT / TAX
    // ═════════════════════════════════════════════════════════════════════
    public Guid? VatAccountId { get; set; }
    public string? VatAccountName { get; set; }

    public Guid? VatOutputAccountId { get; set; }
    public string? VatOutputAccountName { get; set; }

    public Guid? VatInputAccountId { get; set; }
    public string? VatInputAccountName { get; set; }

    public Guid? VatSettlementAccountId { get; set; }
    public string? VatSettlementAccountName { get; set; }

    public Guid? WithholdingTaxAccountId { get; set; }
    public string? WithholdingTaxAccountName { get; set; }

    public Guid? VatSuspenseAccountId { get; set; }
    public string? VatSuspenseAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // PURCHASES
    // ═════════════════════════════════════════════════════════════════════
    public Guid? PurchaseAccountId { get; set; }
    public string? PurchaseAccountName { get; set; }

    public Guid? PurchaseWithoutVatAccountId { get; set; }
    public string? PurchaseWithoutVatAccountName { get; set; }

    public Guid? PurchaseVatAccountId { get; set; }
    public string? PurchaseVatAccountName { get; set; }

    public Guid? PurchaseReturnAccountId { get; set; }
    public string? PurchaseReturnAccountName { get; set; }

    public Guid? PurchaseDiscountAccountId { get; set; }
    public string? PurchaseDiscountAccountName { get; set; }

    public Guid? PurchaseAccrualAccountId { get; set; }
    public string? PurchaseAccrualAccountName { get; set; }

    public Guid? FreightExpenseAccountId { get; set; }
    public string? FreightExpenseAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // INVENTORY
    // ═════════════════════════════════════════════════════════════════════
    public Guid? InventoryAccountId { get; set; }
    public string? InventoryAccountName { get; set; }

    public Guid? InventoryAdjustmentAccountId { get; set; }
    public string? InventoryAdjustmentAccountName { get; set; }

    public Guid? InventoryLossAccountId { get; set; }
    public string? InventoryLossAccountName { get; set; }

    public Guid? InventoryGainAccountId { get; set; }
    public string? InventoryGainAccountName { get; set; }

    public Guid? DamagedInventoryAccountId { get; set; }
    public string? DamagedInventoryAccountName { get; set; }

    public Guid? ExpiredItemsAccountId { get; set; }
    public string? ExpiredItemsAccountName { get; set; }

    public Guid? StockOpeningAccountId { get; set; }
    public string? StockOpeningAccountName { get; set; }

    public Guid? StockClosingAccountId { get; set; }
    public string? StockClosingAccountName { get; set; }

    public Guid? StockTransferAccountId { get; set; }
    public string? StockTransferAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // COGS
    // ═════════════════════════════════════════════════════════════════════
    public Guid? CogsAccountId { get; set; }
    public string? CogsAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // CASH / POS / BANK
    // ═════════════════════════════════════════════════════════════════════
    public Guid? CashAccountId { get; set; }
    public string? CashAccountName { get; set; }

    public Guid? PosAccountId { get; set; }
    public string? PosAccountName { get; set; }

    public Guid? PettyCashAccountId { get; set; }
    public string? PettyCashAccountName { get; set; }

    public Guid? CashDifferenceAccountId { get; set; }
    public string? CashDifferenceAccountName { get; set; }

    public Guid? BankAccountId { get; set; }
    public string? BankAccountName { get; set; }

    public Guid? BankFeesAccountId { get; set; }
    public string? BankFeesAccountName { get; set; }

    public Guid? ChequeAccountId { get; set; }
    public string? ChequeAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // CUSTOMER / SUPPLIER
    // ═════════════════════════════════════════════════════════════════════
    public Guid? ReceivableAccountId { get; set; }
    public string? ReceivableAccountName { get; set; }

    public Guid? CustomerAdvanceAccountId { get; set; }
    public string? CustomerAdvanceAccountName { get; set; }

    public Guid? CustomerRefundAccountId { get; set; }
    public string? CustomerRefundAccountName { get; set; }

    public Guid? SupplierAdvanceAccountId { get; set; }
    public string? SupplierAdvanceAccountName { get; set; }

    public Guid? SupplierPayableAccountId { get; set; }
    public string? SupplierPayableAccountName { get; set; }

    public Guid? BadDebtAccountId { get; set; }
    public string? BadDebtAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // EXPENSES
    // ═════════════════════════════════════════════════════════════════════
    public Guid? GeneralExpenseAccountId { get; set; }
    public string? GeneralExpenseAccountName { get; set; }

    public Guid? SalaryExpenseAccountId { get; set; }
    public string? SalaryExpenseAccountName { get; set; }

    public Guid? RentExpenseAccountId { get; set; }
    public string? RentExpenseAccountName { get; set; }

    public Guid? ElectricityExpenseAccountId { get; set; }
    public string? ElectricityExpenseAccountName { get; set; }

    public Guid? InternetExpenseAccountId { get; set; }
    public string? InternetExpenseAccountName { get; set; }

    // ═════════════════════════════════════════════════════════════════════
    // SYSTEM
    // ═════════════════════════════════════════════════════════════════════
    public Guid? RoundOffAccountId { get; set; }
    public string? RoundOffAccountName { get; set; }

    public Guid? ExchangeRateDifferenceAccountId { get; set; }
    public string? ExchangeRateDifferenceAccountName { get; set; }

    public Guid? YearEndClosingAccountId { get; set; }
    public string? YearEndClosingAccountName { get; set; }

    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateAccountingSettingsDto
{
    public Guid BranchId { get; set; }

    // SALES
    public Guid? SalesAccountId { get; set; }
    public Guid? SalesWithoutVatAccountId { get; set; }
    public Guid? SalesReturnAccountId { get; set; }
    public Guid? SalesDiscountAccountId { get; set; }
    public Guid? ZeroRatedSalesAccountId { get; set; }
    public Guid? ExemptSalesAccountId { get; set; }
    public Guid? DeferredRevenueAccountId { get; set; }
    public Guid? LoyaltyPointsAccountId { get; set; }
    public Guid? GiftCardAccountId { get; set; }
    public Guid? SalesCommissionAccountId { get; set; }

    // VAT / TAX
    public Guid? VatAccountId { get; set; }
    public Guid? VatOutputAccountId { get; set; }
    public Guid? VatInputAccountId { get; set; }
    public Guid? VatSettlementAccountId { get; set; }
    public Guid? WithholdingTaxAccountId { get; set; }
    public Guid? VatSuspenseAccountId { get; set; }

    // PURCHASES
    public Guid? PurchaseAccountId { get; set; }
    public Guid? PurchaseWithoutVatAccountId { get; set; }
    public Guid? PurchaseVatAccountId { get; set; }
    public Guid? PurchaseReturnAccountId { get; set; }
    public Guid? PurchaseDiscountAccountId { get; set; }
    public Guid? PurchaseAccrualAccountId { get; set; }
    public Guid? FreightExpenseAccountId { get; set; }

    // INVENTORY
    public Guid? InventoryAccountId { get; set; }
    public Guid? InventoryAdjustmentAccountId { get; set; }
    public Guid? InventoryLossAccountId { get; set; }
    public Guid? InventoryGainAccountId { get; set; }
    public Guid? DamagedInventoryAccountId { get; set; }
    public Guid? ExpiredItemsAccountId { get; set; }
    public Guid? StockOpeningAccountId { get; set; }
    public Guid? StockClosingAccountId { get; set; }
    public Guid? StockTransferAccountId { get; set; }

    // COGS
    public Guid? CogsAccountId { get; set; }

    // CASH / POS / BANK
    public Guid? CashAccountId { get; set; }
    public Guid? PosAccountId { get; set; }
    public Guid? PettyCashAccountId { get; set; }
    public Guid? CashDifferenceAccountId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? BankFeesAccountId { get; set; }
    public Guid? ChequeAccountId { get; set; }

    // CUSTOMER / SUPPLIER
    public Guid? ReceivableAccountId { get; set; }
    public Guid? CustomerAdvanceAccountId { get; set; }
    public Guid? CustomerRefundAccountId { get; set; }
    public Guid? SupplierAdvanceAccountId { get; set; }
    public Guid? SupplierPayableAccountId { get; set; }
    public Guid? BadDebtAccountId { get; set; }

    // EXPENSES
    public Guid? GeneralExpenseAccountId { get; set; }
    public Guid? SalaryExpenseAccountId { get; set; }
    public Guid? RentExpenseAccountId { get; set; }
    public Guid? ElectricityExpenseAccountId { get; set; }
    public Guid? InternetExpenseAccountId { get; set; }

    // SYSTEM
    public Guid? RoundOffAccountId { get; set; }
    public Guid? ExchangeRateDifferenceAccountId { get; set; }
    public Guid? YearEndClosingAccountId { get; set; }
}

public class UpdateAccountingSettingsDto
{
    public Guid Oid { get; set; }
    public Guid BranchId { get; set; }

    // SALES
    public Guid? SalesAccountId { get; set; }
    public Guid? SalesWithoutVatAccountId { get; set; }
    public Guid? SalesReturnAccountId { get; set; }
    public Guid? SalesDiscountAccountId { get; set; }
    public Guid? ZeroRatedSalesAccountId { get; set; }
    public Guid? ExemptSalesAccountId { get; set; }
    public Guid? DeferredRevenueAccountId { get; set; }
    public Guid? LoyaltyPointsAccountId { get; set; }
    public Guid? GiftCardAccountId { get; set; }
    public Guid? SalesCommissionAccountId { get; set; }

    // VAT / TAX
    public Guid? VatAccountId { get; set; }
    public Guid? VatOutputAccountId { get; set; }
    public Guid? VatInputAccountId { get; set; }
    public Guid? VatSettlementAccountId { get; set; }
    public Guid? WithholdingTaxAccountId { get; set; }
    public Guid? VatSuspenseAccountId { get; set; }

    // PURCHASES
    public Guid? PurchaseAccountId { get; set; }
    public Guid? PurchaseWithoutVatAccountId { get; set; }
    public Guid? PurchaseVatAccountId { get; set; }
    public Guid? PurchaseReturnAccountId { get; set; }
    public Guid? PurchaseDiscountAccountId { get; set; }
    public Guid? PurchaseAccrualAccountId { get; set; }
    public Guid? FreightExpenseAccountId { get; set; }

    // INVENTORY
    public Guid? InventoryAccountId { get; set; }
    public Guid? InventoryAdjustmentAccountId { get; set; }
    public Guid? InventoryLossAccountId { get; set; }
    public Guid? InventoryGainAccountId { get; set; }
    public Guid? DamagedInventoryAccountId { get; set; }
    public Guid? ExpiredItemsAccountId { get; set; }
    public Guid? StockOpeningAccountId { get; set; }
    public Guid? StockClosingAccountId { get; set; }
    public Guid? StockTransferAccountId { get; set; }

    // COGS
    public Guid? CogsAccountId { get; set; }

    // CASH / POS / BANK
    public Guid? CashAccountId { get; set; }
    public Guid? PosAccountId { get; set; }
    public Guid? PettyCashAccountId { get; set; }
    public Guid? CashDifferenceAccountId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? BankFeesAccountId { get; set; }
    public Guid? ChequeAccountId { get; set; }

    // CUSTOMER / SUPPLIER
    public Guid? ReceivableAccountId { get; set; }
    public Guid? CustomerAdvanceAccountId { get; set; }
    public Guid? CustomerRefundAccountId { get; set; }
    public Guid? SupplierAdvanceAccountId { get; set; }
    public Guid? SupplierPayableAccountId { get; set; }
    public Guid? BadDebtAccountId { get; set; }

    // EXPENSES
    public Guid? GeneralExpenseAccountId { get; set; }
    public Guid? SalaryExpenseAccountId { get; set; }
    public Guid? RentExpenseAccountId { get; set; }
    public Guid? ElectricityExpenseAccountId { get; set; }
    public Guid? InternetExpenseAccountId { get; set; }

    // SYSTEM
    public Guid? RoundOffAccountId { get; set; }
    public Guid? ExchangeRateDifferenceAccountId { get; set; }
    public Guid? YearEndClosingAccountId { get; set; }
}
