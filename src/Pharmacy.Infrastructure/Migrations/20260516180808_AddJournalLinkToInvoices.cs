using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJournalLinkToInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FiscalYearId",
                table: "SalesInvoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JournalEntryId",
                table: "SalesInvoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FiscalYearId",
                table: "ReturnInvoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JournalEntryId",
                table: "ReturnInvoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_FiscalYearId",
                table: "SalesInvoices",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_JournalEntryId",
                table: "SalesInvoices",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_FiscalYearId",
                table: "ReturnInvoices",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvoices_JournalEntryId",
                table: "ReturnInvoices",
                column: "JournalEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnInvoices_FiscalYears_FiscalYearId",
                table: "ReturnInvoices",
                column: "FiscalYearId",
                principalSchema: "Accounting",
                principalTable: "FiscalYears",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnInvoices_JournalEntries_JournalEntryId",
                table: "ReturnInvoices",
                column: "JournalEntryId",
                principalSchema: "Accounting",
                principalTable: "JournalEntries",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_FiscalYears_FiscalYearId",
                table: "SalesInvoices",
                column: "FiscalYearId",
                principalSchema: "Accounting",
                principalTable: "FiscalYears",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_JournalEntries_JournalEntryId",
                table: "SalesInvoices",
                column: "JournalEntryId",
                principalSchema: "Accounting",
                principalTable: "JournalEntries",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnInvoices_FiscalYears_FiscalYearId",
                table: "ReturnInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnInvoices_JournalEntries_JournalEntryId",
                table: "ReturnInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_FiscalYears_FiscalYearId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_JournalEntries_JournalEntryId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_FiscalYearId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_JournalEntryId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_ReturnInvoices_FiscalYearId",
                table: "ReturnInvoices");

            migrationBuilder.DropIndex(
                name: "IX_ReturnInvoices_JournalEntryId",
                table: "ReturnInvoices");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "JournalEntryId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                table: "ReturnInvoices");

            migrationBuilder.DropColumn(
                name: "JournalEntryId",
                table: "ReturnInvoices");
        }
    }
}
