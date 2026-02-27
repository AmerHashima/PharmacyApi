using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addstock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Products_ProductId",
                table: "StockTransactions");

            //migrationBuilder.DropIndex(
            //    name: "IX_StockTransactions_ProductId",
            //    table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "StockTransactions");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "StockTransactions",
                newName: "ApprovedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "StockTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationId",
                table: "StockTransactions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductOid",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockTransactionDetails",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Gtin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_StockTransactionDetails", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_StockTransactionDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactionDetails_StockTransactions_StockTransactionId",
                        column: x => x.StockTransactionId,
                        principalTable: "StockTransactions",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ProductOid",
                table: "StockTransactions",
                column: "ProductOid");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionDetails_ProductId",
                table: "StockTransactionDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionDetails_StockTransactionId",
                table: "StockTransactionDetails",
                column: "StockTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Products_ProductOid",
                table: "StockTransactions",
                column: "ProductOid",
                principalTable: "Products",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Products_ProductOid",
                table: "StockTransactions");

            migrationBuilder.DropTable(
                name: "StockTransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_ProductOid",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "ProductOid",
                table: "StockTransactions");

            migrationBuilder.RenameColumn(
                name: "ApprovedDate",
                table: "StockTransactions",
                newName: "ExpiryDate");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "StockTransactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "StockTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "StockTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "StockTransactions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitCost",
                table: "StockTransactions",
                type: "decimal(18,2)",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_StockTransactions_ProductId",
            //    table: "StockTransactions",
            //    column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Products_ProductId",
                table: "StockTransactions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
