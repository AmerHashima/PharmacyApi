using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Integration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuildingNumber",
                table: "Branches",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CRN",
                table: "Branches",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "Branches",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitySubdivisionName",
                table: "Branches",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentifyLookupId",
                table: "Branches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentifyValue",
                table: "Branches",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalZone",
                table: "Branches",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationName",
                table: "Branches",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "Branches",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VatNumber",
                table: "Branches",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_IdentifyLookupId",
                table: "Branches",
                column: "IdentifyLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AppLookupDetail_IdentifyLookupId",
                table: "Branches",
                column: "IdentifyLookupId",
                principalTable: "AppLookupDetail",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AppLookupDetail_IdentifyLookupId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_IdentifyLookupId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CRN",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CitySubdivisionName",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "IdentifyLookupId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "IdentifyValue",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "PostalZone",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "RegistrationName",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "VatNumber",
                table: "Branches");
        }
    }
}
