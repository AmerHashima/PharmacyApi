using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accounting");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountNameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AccountNameEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountLevel = table.Column<int>(type: "int", nullable: false),
                    AccountTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsLeaf = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Accounts", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Accounts_AppLookupDetail_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Accounts_AppLookupDetail_NatureId",
                        column: x => x.NatureId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_CostCenters", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_CostCenters_CostCenters_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Accounting",
                        principalTable: "CostCenters",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "FiscalYears",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_FiscalYears", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrencyCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_BankAccounts", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_AppLookupDetail_CurrencyCodeId",
                        column: x => x.CurrencyCodeId,
                        principalTable: "AppLookupDetail",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashBoxes",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_CashBoxes", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_CashBoxes_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashBoxes_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FiscalYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    IsReversed = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_JournalEntries", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_FiscalYears_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalSchema: "Accounting",
                        principalTable: "FiscalYears",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryDetails",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_JournalEntryDetails", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_JournalEntryDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Accounting",
                        principalTable: "Accounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryDetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Accounting",
                        principalTable: "CostCenters",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryDetails_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntries",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVouchers",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StakeholderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_PaymentVouchers", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "Accounting",
                        principalTable: "BankAccounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_CashBoxes_CashBoxId",
                        column: x => x.CashBoxId,
                        principalSchema: "Accounting",
                        principalTable: "CashBoxes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntries",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_Stakeholders_StakeholderId",
                        column: x => x.StakeholderId,
                        principalTable: "Stakeholders",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptVouchers",
                schema: "Accounting",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_ReceiptVouchers", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ReceiptVouchers_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "Accounting",
                        principalTable: "BankAccounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptVouchers_CashBoxes_CashBoxId",
                        column: x => x.CashBoxId,
                        principalSchema: "Accounting",
                        principalTable: "CashBoxes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptVouchers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptVouchers_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntries",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountCode",
                schema: "Accounting",
                table: "Accounts",
                column: "AccountCode",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                schema: "Accounting",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_NatureId",
                schema: "Accounting",
                table: "Accounts",
                column: "NatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentId",
                schema: "Accounting",
                table: "Accounts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_AccountId",
                schema: "Accounting",
                table: "BankAccounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BranchId",
                schema: "Accounting",
                table: "BankAccounts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CurrencyCodeId",
                schema: "Accounting",
                table: "BankAccounts",
                column: "CurrencyCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxes_AccountId",
                schema: "Accounting",
                table: "CashBoxes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxes_BranchId",
                schema: "Accounting",
                table: "CashBoxes",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_ParentId",
                schema: "Accounting",
                table: "CostCenters",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_BranchId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_EntryNumber",
                schema: "Accounting",
                table: "JournalEntries",
                column: "EntryNumber",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_FiscalYearId",
                schema: "Accounting",
                table: "JournalEntries",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryDetails_AccountId",
                schema: "Accounting",
                table: "JournalEntryDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryDetails_CostCenterId",
                schema: "Accounting",
                table: "JournalEntryDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryDetails_JournalEntryId",
                schema: "Accounting",
                table: "JournalEntryDetails",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_BankAccountId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_CashBoxId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_JournalEntryId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_StakeholderId",
                schema: "Accounting",
                table: "PaymentVouchers",
                column: "StakeholderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_BankAccountId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_CashBoxId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_CustomerId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_JournalEntryId",
                schema: "Accounting",
                table: "ReceiptVouchers",
                column: "JournalEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalEntryDetails",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "PaymentVouchers",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ReceiptVouchers",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "CostCenters",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "BankAccounts",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "CashBoxes",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "JournalEntries",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "FiscalYears",
                schema: "Accounting");
        }
    }
}
