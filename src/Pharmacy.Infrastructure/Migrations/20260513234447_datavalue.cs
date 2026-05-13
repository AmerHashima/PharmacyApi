using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class datavalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptVouchers_Customers_CustomerId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptVouchers_CustomerId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVouchers_Stakeholders_StakeholderId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropIndex(
                name: "IX_PaymentVouchers_StakeholderId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropColumn(
                name: "StakeholderId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.AddColumn<Guid>(
                name: "FiscalYearId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FiscalYearId",
                schema: "Accounting",
                table: "PaymentVouchers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_FiscalYearId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_FiscalYearId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "FiscalYearId");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceInvoiceId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                schema: "Accounting",
                table: "PaymentVouchers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceInvoiceId",
                schema: "Accounting",
                table: "PaymentVoucherDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StakeholderId",
                schema: "Accounting",
                table: "PaymentVoucherDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_BranchId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVoucherDetails_CustomerId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_BranchId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVoucherDetails_StakeholderId",
                schema: "Accounting",
                table: "PaymentVoucherDetails",
                column: "StakeholderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentVoucherDetails_Stakeholders_StakeholderId",
                schema: "Accounting",
                table: "PaymentVoucherDetails",
                column: "StakeholderId",
                principalTable: "Stakeholders",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentVouchers_Branches_BranchId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentVouchers_FiscalYears_FiscalYearId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "FiscalYearId",
                principalSchema: "Accounting",
                principalTable: "FiscalYears",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptVoucherDetails_Customers_CustomerId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptVouchers_Branches_BranchId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptVouchers_FiscalYears_FiscalYearId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "FiscalYearId",
                principalSchema: "Accounting",
                principalTable: "FiscalYears",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVoucherDetails_Stakeholders_StakeholderId",
                schema: "Accounting",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVouchers_Branches_BranchId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVouchers_FiscalYears_FiscalYearId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptVoucherDetails_Customers_CustomerId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptVouchers_Branches_BranchId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptVouchers_FiscalYears_FiscalYearId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptVouchers_BranchId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptVoucherDetails_CustomerId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentVouchers_BranchId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropIndex(
                name: "IX_PaymentVoucherDetails_StakeholderId",
                schema: "Accounting",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails");

            migrationBuilder.DropColumn(
                name: "ReferenceInvoiceId",
                schema: "Accounting",
                table: "ReceiptVoucherDetails");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropColumn(
                name: "ReferenceInvoiceId",
                schema: "Accounting",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropColumn(
                name: "StakeholderId",
                schema: "Accounting",
                table: "PaymentVoucherDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVouchers_FiscalYears_FiscalYearId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptVouchers_FiscalYears_FiscalYearId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptVouchers_FiscalYearId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropIndex(
                name: "IX_PaymentVouchers_FiscalYearId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                schema: "Accounting",
                table: "ReceiptVouchers");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                schema: "Accounting",
                table: "PaymentVouchers");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StakeholderId",
                schema: "Accounting",
                table: "PaymentVouchers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_CustomerId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_StakeholderId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "StakeholderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentVouchers_Stakeholders_StakeholderId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "StakeholderId",
                principalTable: "Stakeholders",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptVouchers_Customers_CustomerId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
