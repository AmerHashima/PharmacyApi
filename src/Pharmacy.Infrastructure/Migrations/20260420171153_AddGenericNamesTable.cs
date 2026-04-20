using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenericNamesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GenericNameId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GenericNames",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericNames", x => x.Oid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_GenericNameId",
                table: "Products",
                column: "GenericNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenericNames_GenericNameId",
                table: "Products",
                column: "GenericNameId",
                principalTable: "GenericNames",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenericNames_GenericNameId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "GenericNames");

            migrationBuilder.DropIndex(
                name: "IX_Products_GenericNameId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GenericNameId",
                table: "Products");
        }
    }
}
