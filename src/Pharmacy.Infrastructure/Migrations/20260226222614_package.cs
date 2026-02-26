using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class package : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageType",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "DosageFormId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageTypeId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DosageFormId",
                table: "Products",
                column: "DosageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PackageTypeId",
                table: "Products",
                column: "PackageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AppLookupDetail_DosageFormId",
                table: "Products",
                column: "DosageFormId",
                principalTable: "AppLookupDetail",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AppLookupDetail_PackageTypeId",
                table: "Products",
                column: "PackageTypeId",
                principalTable: "AppLookupDetail",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AppLookupDetail_DosageFormId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AppLookupDetail_PackageTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DosageFormId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PackageTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DosageFormId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackageTypeId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "PackageType",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
