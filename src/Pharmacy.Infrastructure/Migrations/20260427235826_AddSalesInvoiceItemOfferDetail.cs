using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesInvoiceItemOfferDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OfferDetailId",
                table: "SalesInvoiceItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfferNameSnapshot",
                table: "SalesInvoiceItems",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceItems_OfferDetailId",
                table: "SalesInvoiceItems",
                column: "OfferDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItems_OfferDetails_OfferDetailId",
                table: "SalesInvoiceItems",
                column: "OfferDetailId",
                principalTable: "OfferDetails",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceItems_OfferDetails_OfferDetailId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoiceItems_OfferDetailId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropColumn(
                name: "OfferDetailId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropColumn(
                name: "OfferNameSnapshot",
                table: "SalesInvoiceItems");
        }
    }
}
