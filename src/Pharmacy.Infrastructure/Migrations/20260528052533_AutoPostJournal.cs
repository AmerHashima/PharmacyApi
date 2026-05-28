using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutoPostJournal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JournalEntryId",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AutoPostJournal",
                table: "Branches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_JournalEntryId",
                table: "StockTransactions",
                column: "JournalEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_JournalEntries_JournalEntryId",
                table: "StockTransactions",
                column: "JournalEntryId",
                principalSchema: "Accounting",
                principalTable: "JournalEntries",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_JournalEntries_JournalEntryId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_JournalEntryId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "JournalEntryId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "AutoPostJournal",
                table: "Branches");
        }
    }
}
