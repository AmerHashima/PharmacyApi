using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChildAccountToCashBoxAndBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Accounts_AccountId",
                schema: "Accounting",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_CashBoxes_Accounts_AccountId",
                schema: "Accounting",
                table: "CashBoxes");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                schema: "Accounting",
                table: "CashBoxes",
                newName: "ParentAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CashBoxes_AccountId",
                schema: "Accounting",
                table: "CashBoxes",
                newName: "IX_CashBoxes_ParentAccountId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                schema: "Accounting",
                table: "BankAccounts",
                newName: "ParentAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_AccountId",
                schema: "Accounting",
                table: "BankAccounts",
                newName: "IX_BankAccounts_ParentAccountId");

            migrationBuilder.AddColumn<Guid>(
                name: "ChildAccountId",
                schema: "Accounting",
                table: "CashBoxes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChildAccountId",
                schema: "Accounting",
                table: "BankAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxes_ChildAccountId",
                schema: "Accounting",
                table: "CashBoxes",
                column: "ChildAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ChildAccountId",
                schema: "Accounting",
                table: "BankAccounts",
                column: "ChildAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Accounts_ChildAccountId",
                schema: "Accounting",
                table: "BankAccounts",
                column: "ChildAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Accounts_ParentAccountId",
                schema: "Accounting",
                table: "BankAccounts",
                column: "ParentAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashBoxes_Accounts_ChildAccountId",
                schema: "Accounting",
                table: "CashBoxes",
                column: "ChildAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashBoxes_Accounts_ParentAccountId",
                schema: "Accounting",
                table: "CashBoxes",
                column: "ParentAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Accounts_ChildAccountId",
                schema: "Accounting",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Accounts_ParentAccountId",
                schema: "Accounting",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_CashBoxes_Accounts_ChildAccountId",
                schema: "Accounting",
                table: "CashBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_CashBoxes_Accounts_ParentAccountId",
                schema: "Accounting",
                table: "CashBoxes");

            migrationBuilder.DropIndex(
                name: "IX_CashBoxes_ChildAccountId",
                schema: "Accounting",
                table: "CashBoxes");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_ChildAccountId",
                schema: "Accounting",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "ChildAccountId",
                schema: "Accounting",
                table: "CashBoxes");

            migrationBuilder.DropColumn(
                name: "ChildAccountId",
                schema: "Accounting",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "ParentAccountId",
                schema: "Accounting",
                table: "CashBoxes",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CashBoxes_ParentAccountId",
                schema: "Accounting",
                table: "CashBoxes",
                newName: "IX_CashBoxes_AccountId");

            migrationBuilder.RenameColumn(
                name: "ParentAccountId",
                schema: "Accounting",
                table: "BankAccounts",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_ParentAccountId",
                schema: "Accounting",
                table: "BankAccounts",
                newName: "IX_BankAccounts_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Accounts_AccountId",
                schema: "Accounting",
                table: "BankAccounts",
                column: "AccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashBoxes_Accounts_AccountId",
                schema: "Accounting",
                table: "CashBoxes",
                column: "AccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
