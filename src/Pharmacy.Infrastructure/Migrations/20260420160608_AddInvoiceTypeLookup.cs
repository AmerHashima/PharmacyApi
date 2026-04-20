using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceTypeLookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceTypeId",
                table: "InvoiceSetups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSetups_InvoiceTypeId",
                table: "InvoiceSetups",
                column: "InvoiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceSetups_AppLookupDetail_InvoiceTypeId",
                table: "InvoiceSetups",
                column: "InvoiceTypeId",
                principalTable: "AppLookupDetail",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);

            // ── Seed AppLookupMaster: INVOICE_TYPE ──────────────────────────────
            var now = DateTime.UtcNow;
            var masterInvoiceTypeId = new Guid("11111111-1111-1111-1111-000000000010");
            migrationBuilder.InsertData(
                table: "AppLookupMaster",
                columns: ["Oid", "LookupCode", "LookupNameAr", "LookupNameEn", "IsSystem", "IsDeleted", "CreatedAt"],
                values: new object[] { masterInvoiceTypeId, "INVOICE_TYPE", "نوع الفاتورة", "Invoice Type", true, false, now });

            // ── Seed 4 AppLookupDetail rows ─────────────────────────────────────
            migrationBuilder.InsertData(
                table: "AppLookupDetail",
                columns: ["Oid", "MasterID", "ValueCode", "ValueNameAr", "ValueNameEn", "SortOrder", "IsDefault", "IsActive", "IsDeleted", "CreatedAt"],
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-000000000001"), masterInvoiceTypeId, "POS_INVOICE",             "فاتورة نقاط البيع",        "POS Invoice",             1, true,  true, false, now },
                    { new Guid("22222222-2222-2222-2222-000000000002"), masterInvoiceTypeId, "RETURN_POS_INVOICE",      "مرتجع فاتورة نقاط البيع", "Return POS Invoice",      2, false, true, false, now },
                    { new Guid("22222222-2222-2222-2222-000000000003"), masterInvoiceTypeId, "SUPPLIER_INVOICE",        "فاتورة المورد",            "Supplier Invoice",        3, false, true, false, now },
                    { new Guid("22222222-2222-2222-2222-000000000004"), masterInvoiceTypeId, "RETURN_SUPPLIER_INVOICE", "مرتجع فاتورة المورد",     "Return Supplier Invoice", 4, false, true, false, now },
                });

            // ── Update the 4 global InvoiceSetup seed rows with InvoiceTypeId ──
            migrationBuilder.Sql(@"
                UPDATE InvoiceSetups SET InvoiceTypeId = '22222222-2222-2222-2222-000000000001' WHERE Format = 'PosInv'        AND BranchId IS NULL;
                UPDATE InvoiceSetups SET InvoiceTypeId = '22222222-2222-2222-2222-000000000002' WHERE Format = 'ReturnPosInv'  AND BranchId IS NULL;
                UPDATE InvoiceSetups SET InvoiceTypeId = '22222222-2222-2222-2222-000000000003' WHERE Format = 'SupplierInv'   AND BranchId IS NULL;
                UPDATE InvoiceSetups SET InvoiceTypeId = '22222222-2222-2222-2222-000000000004' WHERE Format = 'ReturnSuppInv' AND BranchId IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Clear InvoiceTypeId references before dropping FK
            migrationBuilder.Sql("UPDATE InvoiceSetups SET InvoiceTypeId = NULL;");

            // Remove seeded lookup data
            migrationBuilder.DeleteData("AppLookupDetail", "Oid", new Guid("22222222-2222-2222-2222-000000000001"));
            migrationBuilder.DeleteData("AppLookupDetail", "Oid", new Guid("22222222-2222-2222-2222-000000000002"));
            migrationBuilder.DeleteData("AppLookupDetail", "Oid", new Guid("22222222-2222-2222-2222-000000000003"));
            migrationBuilder.DeleteData("AppLookupDetail", "Oid", new Guid("22222222-2222-2222-2222-000000000004"));
            migrationBuilder.DeleteData("AppLookupMaster", "Oid", new Guid("11111111-1111-1111-1111-000000000010"));

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceSetups_AppLookupDetail_InvoiceTypeId",
                table: "InvoiceSetups");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceSetups_InvoiceTypeId",
                table: "InvoiceSetups");

            migrationBuilder.DropColumn(
                name: "InvoiceTypeId",
                table: "InvoiceSetups");
        }
    }
}
