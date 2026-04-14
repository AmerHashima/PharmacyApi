using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class storeid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_StoreId",
                table: "StockTransactions",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Stores_StoreId",
                table: "StockTransactions",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Stores_StoreId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_StoreId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "StockTransactions");
        }
    }
}
