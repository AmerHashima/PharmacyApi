using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvocieBinarySecuirtyKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IntegrationValue",
                table: "BranchIntegrationSettings",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IntegrationValue",
                table: "BranchIntegrationSettings",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2147483647,
                oldNullable: true);
        }
    }
}
