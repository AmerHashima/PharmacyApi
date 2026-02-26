using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class productdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrugNameAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "VatTypeId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_VatTypeId",
                table: "Products",
                column: "VatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AppLookupDetail_VatTypeId",
                table: "Products",
                column: "VatTypeId",
                principalTable: "AppLookupDetail",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AppLookupDetail_VatTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_VatTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DrugNameAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VatTypeId",
                table: "Products");
        }
    }
}
