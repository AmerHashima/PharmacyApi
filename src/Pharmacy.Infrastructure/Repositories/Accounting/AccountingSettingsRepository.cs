using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class AccountingSettingsRepository : BaseRepository<AccountingSettings>, IAccountingSettingsRepository
{
    public AccountingSettingsRepository(PharmacyDbContext context) : base(context) { }

    public async Task<AccountingSettings?> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Branch)
            // Sales
            .Include(s => s.SalesAccount)
            .Include(s => s.SalesWithoutVatAccount)
            .Include(s => s.SalesReturnAccount)
            .Include(s => s.SalesDiscountAccount)
            .Include(s => s.ZeroRatedSalesAccount)
            .Include(s => s.ExemptSalesAccount)
            .Include(s => s.DeferredRevenueAccount)
            .Include(s => s.LoyaltyPointsAccount)
            .Include(s => s.GiftCardAccount)
            .Include(s => s.SalesCommissionAccount)
            // VAT / Tax
            .Include(s => s.VatAccount)
            .Include(s => s.VatOutputAccount)
            .Include(s => s.VatInputAccount)
            .Include(s => s.VatSettlementAccount)
            .Include(s => s.WithholdingTaxAccount)
            .Include(s => s.VatSuspenseAccount)
            // Purchases
            .Include(s => s.PurchaseAccount)
            .Include(s => s.PurchaseWithoutVatAccount)
            .Include(s => s.PurchaseVatAccount)
            .Include(s => s.PurchaseReturnAccount)
            .Include(s => s.PurchaseDiscountAccount)
            .Include(s => s.PurchaseAccrualAccount)
            .Include(s => s.FreightExpenseAccount)
            // Inventory
            .Include(s => s.InventoryAccount)
            .Include(s => s.InventoryAdjustmentAccount)
            .Include(s => s.InventoryLossAccount)
            .Include(s => s.InventoryGainAccount)
            .Include(s => s.DamagedInventoryAccount)
            .Include(s => s.ExpiredItemsAccount)
            .Include(s => s.StockOpeningAccount)
            .Include(s => s.StockClosingAccount)
            .Include(s => s.StockTransferAccount)
            // COGS
            .Include(s => s.CogsAccount)
            // Cash / POS / Bank
            .Include(s => s.CashAccount)
            .Include(s => s.PosAccount)
            .Include(s => s.PettyCashAccount)
            .Include(s => s.CashDifferenceAccount)
            .Include(s => s.BankAccount)
            .Include(s => s.BankFeesAccount)
            .Include(s => s.ChequeAccount)
            // Customer / Supplier
            .Include(s => s.ReceivableAccount)
            .Include(s => s.CustomerAdvanceAccount)
            .Include(s => s.CustomerRefundAccount)
            .Include(s => s.SupplierAdvanceAccount)
            .Include(s => s.SupplierPayableAccount)
            .Include(s => s.BadDebtAccount)
            // Expenses
            .Include(s => s.GeneralExpenseAccount)
            .Include(s => s.SalaryExpenseAccount)
            .Include(s => s.RentExpenseAccount)
            .Include(s => s.ElectricityExpenseAccount)
            .Include(s => s.InternetExpenseAccount)
            // System
            .Include(s => s.RoundOffAccount)
            .Include(s => s.ExchangeRateDifferenceAccount)
            .Include(s => s.YearEndClosingAccount)
            .Where(s => s.BranchId == branchId && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
