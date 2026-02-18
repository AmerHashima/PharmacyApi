using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ROle2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_Roles_RoleId",
                table: "SystemUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "SystemUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_Roles_RoleId",
                table: "SystemUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemUsers_Roles_RoleId",
                table: "SystemUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "SystemUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemUsers_Roles_RoleId",
                table: "SystemUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
