using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJournalEntryReferenceTypeFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceType",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceTypeId",
                schema: "Accounting",
                table: "JournalEntries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_ReferenceTypeId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "ReferenceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_AppLookupDetail_ReferenceTypeId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "ReferenceTypeId",
                principalTable: "AppLookupDetail",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_AppLookupDetail_ReferenceTypeId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_ReferenceTypeId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeId",
                schema: "Accounting",
                table: "JournalEntries");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceType",
                schema: "Accounting",
                table: "JournalEntries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
