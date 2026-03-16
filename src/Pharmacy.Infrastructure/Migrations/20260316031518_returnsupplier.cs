using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class returnsupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockTransactionReturns",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ToBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NotificationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReturnInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OriginalTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_StockTransactionReturns", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_StockTransactionReturns_AppLookupDetail_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_StockTransactionReturns_Branches_FromBranchId",
                        column: x => x.FromBranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransactionReturns_Branches_ToBranchId",
                        column: x => x.ToBranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransactionReturns_ReturnInvoices_ReturnInvoiceId",
                        column: x => x.ReturnInvoiceId,
                        principalTable: "ReturnInvoices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransactionReturns_Stakeholders_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Stakeholders",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_StockTransactionReturns_StockTransactions_OriginalTransactionId",
                        column: x => x.OriginalTransactionId,
                        principalTable: "StockTransactions",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactionReturnDetails",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockTransactionReturnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Gtin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_StockTransactionReturnDetails", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_StockTransactionReturnDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactionReturnDetails_StockTransactionReturns_StockTransactionReturnId",
                        column: x => x.StockTransactionReturnId,
                        principalTable: "StockTransactionReturns",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturnDetails_ProductId",
                table: "StockTransactionReturnDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturnDetails_StockTransactionReturnId",
                table: "StockTransactionReturnDetails",
                column: "StockTransactionReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturns_FromBranchId",
                table: "StockTransactionReturns",
                column: "FromBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturns_OriginalTransactionId",
                table: "StockTransactionReturns",
                column: "OriginalTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturns_ReturnInvoiceId",
                table: "StockTransactionReturns",
                column: "ReturnInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturns_SupplierId",
                table: "StockTransactionReturns",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturns_ToBranchId",
                table: "StockTransactionReturns",
                column: "ToBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionReturns_TransactionTypeId",
                table: "StockTransactionReturns",
                column: "TransactionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTransactionReturnDetails");

            migrationBuilder.DropTable(
                name: "StockTransactionReturns");
        }
    }
}
