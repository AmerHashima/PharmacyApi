using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingSettings",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VatAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscountAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CogsAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InventoryAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceivableAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_AccountingSettings", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_CashAccountId",
                        column: x => x.CashAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_CogsAccountId",
                        column: x => x.CogsAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_DiscountAccountId",
                        column: x => x.DiscountAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_InventoryAccountId",
                        column: x => x.InventoryAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_ReceivableAccountId",
                        column: x => x.ReceivableAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_SalesAccountId",
                        column: x => x.SalesAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Accounts_VatAccountId",
                        column: x => x.VatAccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_BankAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_BranchId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_CashAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CashAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_CogsAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "CogsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_DiscountAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "DiscountAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_InventoryAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "InventoryAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_ReceivableAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "ReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SalesAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "SalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_VatAccountId",
                schema: "Accounting",
                table: "AccountingSettings",
                column: "VatAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingSettings",
                schema: "Accounting");
        }
    }
}
