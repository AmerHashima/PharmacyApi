using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseInvoicesCashierShiftsSalesInvoicePayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JournalEntryId",
                table: "StockTransactionReturns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CashierShifts",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CashBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpectedBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierShifts", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_CashierShifts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashierShifts_CashBoxes_CashBoxId",
                        column: x => x.CashBoxId,
                        principalSchema: "Accounting",
                        principalTable: "CashBoxes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashierShifts_SystemUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoices",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierInvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StockTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FiscalYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoices", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_AppLookupDetail_InvoiceStatusId",
                        column: x => x.InvoiceStatusId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_FiscalYears_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalSchema: "Accounting",
                        principalTable: "FiscalYears",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntries",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Stakeholders_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Stakeholders",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_StockTransactions_StockTransactionId",
                        column: x => x.StockTransactionId,
                        principalTable: "StockTransactions",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "CashierShiftDetails",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierShiftDetails", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_CashierShiftDetails_AppLookupDetail_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_CashierShiftDetails_AppLookupDetail_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_CashierShiftDetails_CashierShifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "CashierShifts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoicePayments",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovalCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoicePayments", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_SalesInvoicePayments_AppLookupDetail_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesInvoicePayments_CashierShifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "CashierShifts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_SalesInvoicePayments_SalesInvoices_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoicePayments",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentVoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoicePayments", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoicePayments_AppLookupDetail_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoicePayments_PaymentVouchers_PaymentVoucherId",
                        column: x => x.PaymentVoucherId,
                        principalSchema: "Accounting",
                        principalTable: "PaymentVouchers",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_PurchaseInvoicePayments_PurchaseInvoices_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturns_JournalEntryId",
                table: "StockTransactionReturns",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShiftDetails_PaymentMethodId",
                table: "CashierShiftDetails",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShiftDetails_ShiftId",
                table: "CashierShiftDetails",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShiftDetails_TransactionTypeId",
                table: "CashierShiftDetails",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShifts_BranchId",
                table: "CashierShifts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShifts_CashBoxId",
                table: "CashierShifts",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierShifts_UserId",
                table: "CashierShifts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoicePayments_PaymentMethodId",
                table: "PurchaseInvoicePayments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoicePayments_PaymentVoucherId",
                table: "PurchaseInvoicePayments",
                column: "PaymentVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoicePayments_PurchaseInvoiceId",
                table: "PurchaseInvoicePayments",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_BranchId",
                table: "PurchaseInvoices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_FiscalYearId",
                table: "PurchaseInvoices",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_InvoiceStatusId",
                table: "PurchaseInvoices",
                column: "InvoiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_JournalEntryId",
                table: "PurchaseInvoices",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_StockTransactionId",
                table: "PurchaseInvoices",
                column: "StockTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_SupplierId",
                table: "PurchaseInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoicePayments_PaymentMethodId",
                table: "SalesInvoicePayments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoicePayments_SalesInvoiceId",
                table: "SalesInvoicePayments",
                column: "SalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoicePayments_ShiftId",
                table: "SalesInvoicePayments",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionReturns_JournalEntries_JournalEntryId",
                table: "StockTransactionReturns",
                column: "JournalEntryId",
                principalSchema: "Accounting",
                principalTable: "JournalEntries",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionReturns_JournalEntries_JournalEntryId",
                table: "StockTransactionReturns");

            migrationBuilder.DropTable(
                name: "CashierShiftDetails");

            migrationBuilder.DropTable(
                name: "PurchaseInvoicePayments");

            migrationBuilder.DropTable(
                name: "SalesInvoicePayments");

            migrationBuilder.DropTable(
                name: "PurchaseInvoices");

            migrationBuilder.DropTable(
                name: "CashierShifts");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactionReturns_JournalEntryId",
                table: "StockTransactionReturns");

            migrationBuilder.DropColumn(
                name: "JournalEntryId",
                table: "StockTransactionReturns");
        }
    }
}
