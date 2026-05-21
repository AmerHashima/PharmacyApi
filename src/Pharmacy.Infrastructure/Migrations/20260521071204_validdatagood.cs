using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class validdatagood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_DiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.RenameColumn(
                name: "DiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                newName: "ZeroRatedSalesAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountingSettings_DiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                newName: "IX_AccountingSettings_ZeroRatedSalesAccountId");

            migrationBuilder.AddColumn<Guid>(
                name: "BadDebtAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankFeesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CashDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChequeAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerRefundAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DamagedInventoryAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeferredRevenueAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ElectricityExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExchangeRateDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExemptSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExpiredItemsAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FreightExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GeneralExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GiftCardAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InternetExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryAdjustmentAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryGainAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryLossAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LoyaltyPointsAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PettyCashAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PosAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseAccrualAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RentExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoundOffAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalaryExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesCommissionAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockOpeningAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StockTransferAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierPayableAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VatInputAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VatOutputAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VatSettlementAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VatSuspenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WithholdingTaxAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "YearEndClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_BadDebtAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "BadDebtAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_BankFeesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "BankFeesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_CashDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CashDifferenceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_ChequeAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ChequeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_CustomerAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CustomerAdvanceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_CustomerRefundAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CustomerRefundAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_DamagedInventoryAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "DamagedInventoryAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_DeferredRevenueAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "DeferredRevenueAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_ElectricityExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ElectricityExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_ExchangeRateDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ExchangeRateDifferenceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_ExemptSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ExemptSalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_ExpiredItemsAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ExpiredItemsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_FreightExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "FreightExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_GeneralExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "GeneralExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_GiftCardAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "GiftCardAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_InternetExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InternetExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_InventoryAdjustmentAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InventoryAdjustmentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_InventoryGainAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InventoryGainAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_InventoryLossAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InventoryLossAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_LoyaltyPointsAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "LoyaltyPointsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PettyCashAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PettyCashAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PosAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PosAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PurchaseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PurchaseAccrualAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseAccrualAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PurchaseDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseDiscountAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PurchaseReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseReturnAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PurchaseVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseVatAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PurchaseWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseWithoutVatAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_RentExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "RentExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_RoundOffAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "RoundOffAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SalaryExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalaryExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SalesCommissionAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesCommissionAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SalesDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesDiscountAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SalesReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesReturnAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SalesWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesWithoutVatAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_StockClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "StockClosingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_StockOpeningAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "StockOpeningAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_StockTransferAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "StockTransferAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SupplierAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SupplierAdvanceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SupplierPayableAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SupplierPayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_VatInputAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatInputAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_VatOutputAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatOutputAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_VatSettlementAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatSettlementAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_VatSuspenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatSuspenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_WithholdingTaxAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "WithholdingTaxAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_YearEndClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "YearEndClosingAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_BadDebtAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "BadDebtAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_BankFeesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "BankFeesAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_CashDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CashDifferenceAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_ChequeAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ChequeAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_CustomerAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CustomerAdvanceAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_CustomerRefundAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CustomerRefundAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_DamagedInventoryAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "DamagedInventoryAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_DeferredRevenueAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "DeferredRevenueAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_ElectricityExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ElectricityExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_ExchangeRateDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ExchangeRateDifferenceAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_ExemptSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ExemptSalesAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_ExpiredItemsAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ExpiredItemsAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_FreightExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "FreightExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_GeneralExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "GeneralExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_GiftCardAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "GiftCardAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_InternetExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InternetExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_InventoryAdjustmentAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InventoryAdjustmentAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_InventoryGainAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InventoryGainAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_InventoryLossAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InventoryLossAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_LoyaltyPointsAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "LoyaltyPointsAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PettyCashAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PettyCashAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PosAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PosAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseAccrualAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseAccrualAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseDiscountAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseReturnAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseVatAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "PurchaseWithoutVatAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_RentExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "RentExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_RoundOffAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "RoundOffAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_SalaryExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalaryExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesCommissionAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesCommissionAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesDiscountAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesReturnAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesWithoutVatAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_StockClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "StockClosingAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_StockOpeningAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "StockOpeningAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_StockTransferAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "StockTransferAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_SupplierAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SupplierAdvanceAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_SupplierPayableAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SupplierPayableAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_VatInputAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatInputAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_VatOutputAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatOutputAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_VatSettlementAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatSettlementAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_VatSuspenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatSuspenseAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_WithholdingTaxAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "WithholdingTaxAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_YearEndClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "YearEndClosingAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_ZeroRatedSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ZeroRatedSalesAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_BadDebtAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_BankFeesAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_CashDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_ChequeAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_CustomerAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_CustomerRefundAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_DamagedInventoryAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_DeferredRevenueAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_ElectricityExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_ExchangeRateDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_ExemptSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_ExpiredItemsAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_FreightExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_GeneralExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_GiftCardAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_InternetExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_InventoryAdjustmentAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_InventoryGainAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_InventoryLossAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_LoyaltyPointsAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PettyCashAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PosAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseAccrualAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_PurchaseWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_RentExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_RoundOffAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_SalaryExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesCommissionAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_SalesWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_StockClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_StockOpeningAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_StockTransferAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_SupplierAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_SupplierPayableAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_VatInputAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_VatOutputAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_VatSettlementAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_VatSuspenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_WithholdingTaxAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_YearEndClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Accounts_ZeroRatedSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_BadDebtAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_BankFeesAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_CashDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_ChequeAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_CustomerAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_CustomerRefundAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_DamagedInventoryAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_DeferredRevenueAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_ElectricityExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_ExchangeRateDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_ExemptSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_ExpiredItemsAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_FreightExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_GeneralExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_GiftCardAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_InternetExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_InventoryAdjustmentAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_InventoryGainAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_InventoryLossAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_LoyaltyPointsAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PettyCashAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PosAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PurchaseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PurchaseAccrualAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PurchaseDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PurchaseReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PurchaseVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_PurchaseWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_RentExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_RoundOffAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_SalaryExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_SalesCommissionAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_SalesDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_SalesReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_SalesWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_StockClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_StockOpeningAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_StockTransferAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_SupplierAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_SupplierPayableAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_VatInputAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_VatOutputAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_VatSettlementAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_VatSuspenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_WithholdingTaxAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_YearEndClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "BadDebtAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "BankFeesAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "CashDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "ChequeAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "CustomerAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "CustomerRefundAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "DamagedInventoryAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "DeferredRevenueAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "ElectricityExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "ExchangeRateDifferenceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "ExemptSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "ExpiredItemsAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "FreightExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "GeneralExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "GiftCardAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "InternetExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "InventoryAdjustmentAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "InventoryGainAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "InventoryLossAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "LoyaltyPointsAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PettyCashAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PosAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PurchaseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PurchaseAccrualAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PurchaseDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PurchaseReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PurchaseVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "PurchaseWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "RentExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "RoundOffAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "SalaryExpenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "SalesCommissionAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "SalesDiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "SalesReturnAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "SalesWithoutVatAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "StockClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "StockOpeningAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "StockTransferAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "SupplierAdvanceAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "SupplierPayableAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "VatInputAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "VatOutputAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "VatSettlementAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "VatSuspenseAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "WithholdingTaxAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "YearEndClosingAccountId",
                schema: "Accounting",
                table: "AccountingSettings");

            migrationBuilder.RenameColumn(
                name: "ZeroRatedSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                newName: "DiscountAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountingSettings_ZeroRatedSalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                newName: "IX_AccountingSettings_DiscountAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Accounts_DiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "DiscountAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");
        }
    }
}
