using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultBranchAndInvoiceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_Branches_BranchId",
                table: "SystemUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchOid",
                table: "SystemUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultBranchId",
                table: "SystemUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceFormat",
                table: "Branches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNumber",
                table: "Branches",
                type: "int",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_BranchOid",
                table: "SystemUsers",
                column: "BranchOid");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_DefaultBranchId",
                table: "SystemUsers",
                column: "DefaultBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_Branches_BranchId",
                table: "SystemUsers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_Branches_BranchOid",
                table: "SystemUsers",
                column: "BranchOid",
                principalTable: "Branches",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_Branches_DefaultBranchId",
                table: "SystemUsers",
                column: "DefaultBranchId",
                principalTable: "Branches",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_Branches_BranchId",
                table: "SystemUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_Branches_BranchOid",
                table: "SystemUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_Branches_DefaultBranchId",
                table: "SystemUsers");

            migrationBuilder.DropIndex(
                name: "IX_SystemUsers_BranchOid",
                table: "SystemUsers");

            migrationBuilder.DropIndex(
                name: "IX_SystemUsers_DefaultBranchId",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "BranchOid",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "DefaultBranchId",
                table: "SystemUsers");

            migrationBuilder.DropColumn(
                name: "InvoiceFormat",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Branches");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_Branches_BranchId",
                table: "SystemUsers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Oid");
        }
    }
}
