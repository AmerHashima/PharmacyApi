using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountFinalAccountFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FinalAccountId",
                schema: "Accounting",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_FinalAccountId",
                schema: "Accounting",
                table: "Accounts",
                column: "FinalAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AppLookupDetail_FinalAccountId",
                schema: "Accounting",
                table: "Accounts",
                column: "FinalAccountId",
                principalTable: "AppLookupDetail",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AppLookupDetail_FinalAccountId",
                schema: "Accounting",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_FinalAccountId",
                schema: "Accounting",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "FinalAccountId",
                schema: "Accounting",
                table: "Accounts");
        }
    }
}
