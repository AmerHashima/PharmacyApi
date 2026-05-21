using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

/// <summary>
/// Per-branch accounting settings.
/// Maps each accounting event to the correct Chart-of-Accounts entry.
/// </summary>
[Table("AccountingSettings", Schema = "Accounting")]
public class AccountingSettings : BaseEntity
{
    // =====================================================
    // BRANCH
    // =====================================================

    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    // =====================================================
    // SALES
    // =====================================================

    public Guid? SalesAccountId { get; set; }
    [ForeignKey(nameof(SalesAccountId))]
    public virtual Account? SalesAccount { get; set; }

    public Guid? SalesWithoutVatAccountId { get; set; }
    [ForeignKey(nameof(SalesWithoutVatAccountId))]
    public virtual Account? SalesWithoutVatAccount { get; set; }

    public Guid? SalesReturnAccountId { get; set; }
    [ForeignKey(nameof(SalesReturnAccountId))]
    public virtual Account? SalesReturnAccount { get; set; }

    public Guid? SalesDiscountAccountId { get; set; }
    [ForeignKey(nameof(SalesDiscountAccountId))]
    public virtual Account? SalesDiscountAccount { get; set; }

    public Guid? ZeroRatedSalesAccountId { get; set; }
    [ForeignKey(nameof(ZeroRatedSalesAccountId))]
    public virtual Account? ZeroRatedSalesAccount { get; set; }

    public Guid? ExemptSalesAccountId { get; set; }
    [ForeignKey(nameof(ExemptSalesAccountId))]
    public virtual Account? ExemptSalesAccount { get; set; }

    public Guid? DeferredRevenueAccountId { get; set; }
    [ForeignKey(nameof(DeferredRevenueAccountId))]
    public virtual Account? DeferredRevenueAccount { get; set; }

    public Guid? LoyaltyPointsAccountId { get; set; }
    [ForeignKey(nameof(LoyaltyPointsAccountId))]
    public virtual Account? LoyaltyPointsAccount { get; set; }

    public Guid? GiftCardAccountId { get; set; }
    [ForeignKey(nameof(GiftCardAccountId))]
    public virtual Account? GiftCardAccount { get; set; }

    public Guid? SalesCommissionAccountId { get; set; }
    [ForeignKey(nameof(SalesCommissionAccountId))]
    public virtual Account? SalesCommissionAccount { get; set; }

    // =====================================================
    // VAT / TAX
    // =====================================================

    public Guid? VatAccountId { get; set; }
    [ForeignKey(nameof(VatAccountId))]
    public virtual Account? VatAccount { get; set; }

    public Guid? VatOutputAccountId { get; set; }
    [ForeignKey(nameof(VatOutputAccountId))]
    public virtual Account? VatOutputAccount { get; set; }

    public Guid? VatInputAccountId { get; set; }
    [ForeignKey(nameof(VatInputAccountId))]
    public virtual Account? VatInputAccount { get; set; }

    public Guid? VatSettlementAccountId { get; set; }
    [ForeignKey(nameof(VatSettlementAccountId))]
    public virtual Account? VatSettlementAccount { get; set; }

    public Guid? WithholdingTaxAccountId { get; set; }
    [ForeignKey(nameof(WithholdingTaxAccountId))]
    public virtual Account? WithholdingTaxAccount { get; set; }

    public Guid? VatSuspenseAccountId { get; set; }
    [ForeignKey(nameof(VatSuspenseAccountId))]
    public virtual Account? VatSuspenseAccount { get; set; }

    // =====================================================
    // PURCHASES
    // =====================================================

    public Guid? PurchaseAccountId { get; set; }
    [ForeignKey(nameof(PurchaseAccountId))]
    public virtual Account? PurchaseAccount { get; set; }

    public Guid? PurchaseWithoutVatAccountId { get; set; }
    [ForeignKey(nameof(PurchaseWithoutVatAccountId))]
    public virtual Account? PurchaseWithoutVatAccount { get; set; }

    public Guid? PurchaseVatAccountId { get; set; }
    [ForeignKey(nameof(PurchaseVatAccountId))]
    public virtual Account? PurchaseVatAccount { get; set; }

    public Guid? PurchaseReturnAccountId { get; set; }
    [ForeignKey(nameof(PurchaseReturnAccountId))]
    public virtual Account? PurchaseReturnAccount { get; set; }

    public Guid? PurchaseDiscountAccountId { get; set; }
    [ForeignKey(nameof(PurchaseDiscountAccountId))]
    public virtual Account? PurchaseDiscountAccount { get; set; }

    public Guid? PurchaseAccrualAccountId { get; set; }
    [ForeignKey(nameof(PurchaseAccrualAccountId))]
    public virtual Account? PurchaseAccrualAccount { get; set; }

    public Guid? FreightExpenseAccountId { get; set; }
    [ForeignKey(nameof(FreightExpenseAccountId))]
    public virtual Account? FreightExpenseAccount { get; set; }

    // =====================================================
    // INVENTORY
    // =====================================================

    public Guid? InventoryAccountId { get; set; }
    [ForeignKey(nameof(InventoryAccountId))]
    public virtual Account? InventoryAccount { get; set; }

    public Guid? InventoryAdjustmentAccountId { get; set; }
    [ForeignKey(nameof(InventoryAdjustmentAccountId))]
    public virtual Account? InventoryAdjustmentAccount { get; set; }

    public Guid? InventoryLossAccountId { get; set; }
    [ForeignKey(nameof(InventoryLossAccountId))]
    public virtual Account? InventoryLossAccount { get; set; }

    public Guid? InventoryGainAccountId { get; set; }
    [ForeignKey(nameof(InventoryGainAccountId))]
    public virtual Account? InventoryGainAccount { get; set; }

    public Guid? DamagedInventoryAccountId { get; set; }
    [ForeignKey(nameof(DamagedInventoryAccountId))]
    public virtual Account? DamagedInventoryAccount { get; set; }

    public Guid? ExpiredItemsAccountId { get; set; }
    [ForeignKey(nameof(ExpiredItemsAccountId))]
    public virtual Account? ExpiredItemsAccount { get; set; }

    public Guid? StockOpeningAccountId { get; set; }
    [ForeignKey(nameof(StockOpeningAccountId))]
    public virtual Account? StockOpeningAccount { get; set; }

    public Guid? StockClosingAccountId { get; set; }
    [ForeignKey(nameof(StockClosingAccountId))]
    public virtual Account? StockClosingAccount { get; set; }

    public Guid? StockTransferAccountId { get; set; }
    [ForeignKey(nameof(StockTransferAccountId))]
    public virtual Account? StockTransferAccount { get; set; }

    // =====================================================
    // COGS
    // =====================================================

    public Guid? CogsAccountId { get; set; }
    [ForeignKey(nameof(CogsAccountId))]
    public virtual Account? CogsAccount { get; set; }

    // =====================================================
    // CASH / POS / BANK
    // =====================================================

    public Guid? CashAccountId { get; set; }
    [ForeignKey(nameof(CashAccountId))]
    public virtual Account? CashAccount { get; set; }

    public Guid? PosAccountId { get; set; }
    [ForeignKey(nameof(PosAccountId))]
    public virtual Account? PosAccount { get; set; }

    public Guid? PettyCashAccountId { get; set; }
    [ForeignKey(nameof(PettyCashAccountId))]
    public virtual Account? PettyCashAccount { get; set; }

    public Guid? CashDifferenceAccountId { get; set; }
    [ForeignKey(nameof(CashDifferenceAccountId))]
    public virtual Account? CashDifferenceAccount { get; set; }

    public Guid? BankAccountId { get; set; }
    [ForeignKey(nameof(BankAccountId))]
    public virtual Account? BankAccount { get; set; }

    public Guid? BankFeesAccountId { get; set; }
    [ForeignKey(nameof(BankFeesAccountId))]
    public virtual Account? BankFeesAccount { get; set; }

    public Guid? ChequeAccountId { get; set; }
    [ForeignKey(nameof(ChequeAccountId))]
    public virtual Account? ChequeAccount { get; set; }

    // =====================================================
    // CUSTOMER / SUPPLIER
    // =====================================================

    public Guid? ReceivableAccountId { get; set; }
    [ForeignKey(nameof(ReceivableAccountId))]
    public virtual Account? ReceivableAccount { get; set; }

    public Guid? CustomerAdvanceAccountId { get; set; }
    [ForeignKey(nameof(CustomerAdvanceAccountId))]
    public virtual Account? CustomerAdvanceAccount { get; set; }

    public Guid? CustomerRefundAccountId { get; set; }
    [ForeignKey(nameof(CustomerRefundAccountId))]
    public virtual Account? CustomerRefundAccount { get; set; }

    public Guid? SupplierAdvanceAccountId { get; set; }
    [ForeignKey(nameof(SupplierAdvanceAccountId))]
    public virtual Account? SupplierAdvanceAccount { get; set; }

    public Guid? SupplierPayableAccountId { get; set; }
    [ForeignKey(nameof(SupplierPayableAccountId))]
    public virtual Account? SupplierPayableAccount { get; set; }

    public Guid? BadDebtAccountId { get; set; }
    [ForeignKey(nameof(BadDebtAccountId))]
    public virtual Account? BadDebtAccount { get; set; }

    // =====================================================
    // EXPENSES
    // =====================================================

    public Guid? GeneralExpenseAccountId { get; set; }
    [ForeignKey(nameof(GeneralExpenseAccountId))]
    public virtual Account? GeneralExpenseAccount { get; set; }

    public Guid? SalaryExpenseAccountId { get; set; }
    [ForeignKey(nameof(SalaryExpenseAccountId))]
    public virtual Account? SalaryExpenseAccount { get; set; }

    public Guid? RentExpenseAccountId { get; set; }
    [ForeignKey(nameof(RentExpenseAccountId))]
    public virtual Account? RentExpenseAccount { get; set; }

    public Guid? ElectricityExpenseAccountId { get; set; }
    [ForeignKey(nameof(ElectricityExpenseAccountId))]
    public virtual Account? ElectricityExpenseAccount { get; set; }

    public Guid? InternetExpenseAccountId { get; set; }
    [ForeignKey(nameof(InternetExpenseAccountId))]
    public virtual Account? InternetExpenseAccount { get; set; }

    // =====================================================
    // SYSTEM
    // =====================================================

    public Guid? RoundOffAccountId { get; set; }
    [ForeignKey(nameof(RoundOffAccountId))]
    public virtual Account? RoundOffAccount { get; set; }

    public Guid? ExchangeRateDifferenceAccountId { get; set; }
    [ForeignKey(nameof(ExchangeRateDifferenceAccountId))]
    public virtual Account? ExchangeRateDifferenceAccount { get; set; }

    public Guid? YearEndClosingAccountId { get; set; }
    [ForeignKey(nameof(YearEndClosingAccountId))]
    public virtual Account? YearEndClosingAccount { get; set; }
}