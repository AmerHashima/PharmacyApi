using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class vocher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Accounting",
                table: "ReceiptVouchers",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Accounting",
                table: "PaymentVouchers",
                newName: "TotalAmount");

            migrationBuilder.CreateTable(
                name: "PaymentVoucherDetails",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentVoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_PaymentVoucherDetails", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PaymentVoucherDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVoucherDetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Accounting",
                        principalTable: "CostCenters",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVoucherDetails_PaymentVouchers_PaymentVoucherId",
                        column: x => x.PaymentVoucherId,
                        principalSchema: "Accounting",
                        principalTable: "PaymentVouchers",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVoucherDetails",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptVoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_ReceiptVoucherDetails", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherDetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Accounting",
                        principalTable: "CostCenters",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptVoucherDetails_ReceiptVouchers_ReceiptVoucherId",
                        column: x => x.ReceiptVoucherId,
                        principalSchema: "Accounting",
                        principalTable: "ReceiptVouchers",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherDetails_AccountId",
                schema: "Accounting",
                table: "PaymentVoucherDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherDetails_CostCenterId",
                schema: "Accounting",
                table: "PaymentVoucherDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherDetails_PaymentVoucherId",
                schema: "Accounting",
                table: "PaymentVoucherDetails",
                column: "PaymentVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetails_AccountId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetails_CostCenterId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetails_ReceiptVoucherId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails",
                column: "ReceiptVoucherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentVoucherDetails",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ReceiptVoucherDetails",
                schema: "Accounting");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "Accounting",
                table: "ReceiptVouchers",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "Accounting",
                table: "PaymentVouchers",
                newName: "Amount");
        }
    }
}
