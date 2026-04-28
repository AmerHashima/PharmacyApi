using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesInvoiceDoctorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "SalesInvoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_DoctorId",
                table: "SalesInvoices",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Doctors_DoctorId",
                table: "SalesInvoices",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Doctors_DoctorId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_DoctorId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "SalesInvoices");
        }
    }
}
