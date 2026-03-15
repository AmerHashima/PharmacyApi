using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addreturninvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RemainingQuantity",
                table: "SalesInvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ReturnInvoices",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReturnNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OriginalInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReturnReasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_ReturnInvoices", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ReturnInvoices_AppLookupDetail_InvoiceStatusId",
                        column: x => x.InvoiceStatusId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_ReturnInvoices_AppLookupDetail_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_ReturnInvoices_AppLookupDetail_ReturnReasonId",
                        column: x => x.ReturnReasonId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_ReturnInvoices_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnInvoices_SalesInvoices_OriginalInvoiceId",
                        column: x => x.OriginalInvoiceId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnInvoices_SystemUsers_CashierId",
                        column: x => x.CashierId,
                        principalTable: "SystemUsers",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "ReturnInvoiceItems",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReturnInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalInvoiceItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_ReturnInvoiceItems", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ReturnInvoiceItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnInvoiceItems_ReturnInvoices_ReturnInvoiceId",
                        column: x => x.ReturnInvoiceId,
                        principalTable: "ReturnInvoices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnInvoiceItems_SalesInvoiceItems_OriginalInvoiceItemId",
                        column: x => x.OriginalInvoiceItemId,
                        principalTable: "SalesInvoiceItems",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoiceItems_OriginalInvoiceItemId",
                table: "ReturnInvoiceItems",
                column: "OriginalInvoiceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoiceItems_ProductId",
                table: "ReturnInvoiceItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoiceItems_ReturnInvoiceId",
                table: "ReturnInvoiceItems",
                column: "ReturnInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_BranchId",
                table: "ReturnInvoices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_CashierId",
                table: "ReturnInvoices",
                column: "CashierId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_InvoiceStatusId",
                table: "ReturnInvoices",
                column: "InvoiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_OriginalInvoiceId",
                table: "ReturnInvoices",
                column: "OriginalInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_PaymentMethodId",
                table: "ReturnInvoices",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_ReturnNumber",
                table: "ReturnInvoices",
                column: "ReturnNumber",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_ReturnReasonId",
                table: "ReturnInvoices",
                column: "ReturnReasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnInvoiceItems");

            migrationBuilder.DropTable(
                name: "ReturnInvoices");

            migrationBuilder.DropColumn(
                name: "RemainingQuantity",
                table: "SalesInvoiceItems");
        }
    }
}
