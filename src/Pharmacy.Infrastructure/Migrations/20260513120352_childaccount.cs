using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class childaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChildAccountId",
                table: "Stakeholders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentAccountId",
                table: "Stakeholders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChildAccountId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentAccountId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stakeholders_ChildAccountId",
                table: "Stakeholders",
                column: "ChildAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Stakeholders_ParentAccountId",
                table: "Stakeholders",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ChildAccountId",
                table: "Customers",
                column: "ChildAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ParentAccountId",
                table: "Customers",
                column: "ParentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Accounts_ChildAccountId",
                table: "Customers",
                column: "ChildAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Accounts_ParentAccountId",
                table: "Customers",
                column: "ParentAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Stakeholders_Accounts_ChildAccountId",
                table: "Stakeholders",
                column: "ChildAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Stakeholders_Accounts_ParentAccountId",
                table: "Stakeholders",
                column: "ParentAccountId",
                principalSchema: "Accounting",
                principalTable: "Accounts",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Accounts_ChildAccountId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Accounts_ParentAccountId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Stakeholders_Accounts_ChildAccountId",
                table: "Stakeholders");

            migrationBuilder.DropForeignKey(
                name: "FK_Stakeholders_Accounts_ParentAccountId",
                table: "Stakeholders");

            migrationBuilder.DropIndex(
                name: "IX_Stakeholders_ChildAccountId",
                table: "Stakeholders");

            migrationBuilder.DropIndex(
                name: "IX_Stakeholders_ParentAccountId",
                table: "Stakeholders");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ChildAccountId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ParentAccountId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ChildAccountId",
                table: "Stakeholders");

            migrationBuilder.DropColumn(
                name: "ParentAccountId",
                table: "Stakeholders");

            migrationBuilder.DropColumn(
                name: "ChildAccountId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ParentAccountId",
                table: "Customers");
        }
    }
}
