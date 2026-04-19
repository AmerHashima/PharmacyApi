using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceSetupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceFormat",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Branches");

            migrationBuilder.CreateTable(
                name: "InvoiceSetups",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumberValue = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_InvoiceSetups", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_InvoiceSetups_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSetups_BranchId",
                table: "InvoiceSetups",
                column: "BranchId");

            // ── Seed 4 global template rows (BranchId = null) ──────────────────
            var now = DateTime.UtcNow;
            migrationBuilder.InsertData(
                table: "InvoiceSetups",
                columns: ["Oid", "NameAr", "NameEn", "Format", "NumberValue", "BranchId", "IsDeleted", "CreatedAt"],
                values: new object[,]
                {
                    { Guid.NewGuid(), "فاتورة نقاط البيع",          "POS Invoice",                "PosInv",        1, null, false, now },
                    { Guid.NewGuid(), "مرتجع فاتورة نقاط البيع",   "Return POS Invoice",          "ReturnPosInv",  1, null, false, now },
                    { Guid.NewGuid(), "فاتورة المورد",               "Supplier Invoice",            "SupplierInv",   1, null, false, now },
                    { Guid.NewGuid(), "مرتجع فاتورة المورد",        "Return Supplier Invoice",     "ReturnSuppInv", 1, null, false, now }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceSetups");

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
        }
    }
}
