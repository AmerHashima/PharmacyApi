using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ecommercestore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ecom_Stores",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    ExternalStoreId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PriceListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AutoPullOrders = table.Column<bool>(type: "bit", nullable: false),
                    AutoSyncStock = table.Column<bool>(type: "bit", nullable: false),
                    AutoSyncPrice = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Ecom_Stores", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "Ecom_SyncQueue",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocalEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalEntityId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    MaxRetryCount = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LastTriedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_Ecom_SyncQueue", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "Ecom_WebhookEvents",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessingStatus = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_Ecom_WebhookEvents", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "Ecom_Orders",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    ExternalOrderId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExternalOrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CustomerMobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LocalInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SyncStatus = table.Column<int>(type: "int", nullable: false),
                    LocalStatus = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExternalCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_Ecom_Orders", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Ecom_Orders_Ecom_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Ecom_Stores",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ecom_ProductMappings",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalProductId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExternalVariantId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalSku = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalProductName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LocalProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalProductUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsMapped = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Ecom_ProductMappings", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Ecom_ProductMappings_Ecom_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Ecom_Stores",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ecom_OrderItems",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ECommerceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalProductId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalVariantId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalSku = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LocalProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_Ecom_OrderItems", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Ecom_OrderItems_Ecom_Orders_ECommerceOrderId",
                        column: x => x.ECommerceOrderId,
                        principalTable: "Ecom_Orders",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_OrderItems_ECommerceOrderId",
                table: "Ecom_OrderItems",
                column: "ECommerceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_Orders_ProviderType_ExternalOrderId",
                table: "Ecom_Orders",
                columns: new[] { "ProviderType", "ExternalOrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_Orders_StoreId",
                table: "Ecom_Orders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_ProductMappings_StoreId_ExternalBarcode",
                table: "Ecom_ProductMappings",
                columns: new[] { "StoreId", "ExternalBarcode" });

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_ProductMappings_StoreId_ExternalProductId_ExternalVariantId",
                table: "Ecom_ProductMappings",
                columns: new[] { "StoreId", "ExternalProductId", "ExternalVariantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_ProductMappings_StoreId_IsMapped",
                table: "Ecom_ProductMappings",
                columns: new[] { "StoreId", "IsMapped" });

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_ProductMappings_StoreId_LocalProductId_LocalProductUnitId",
                table: "Ecom_ProductMappings",
                columns: new[] { "StoreId", "LocalProductId", "LocalProductUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_Stores_ProviderType_ExternalStoreId",
                table: "Ecom_Stores",
                columns: new[] { "ProviderType", "ExternalStoreId" },
                unique: true,
                filter: "[ExternalStoreId] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_SyncQueue_Status_ProviderType_EntityType",
                table: "Ecom_SyncQueue",
                columns: new[] { "Status", "ProviderType", "EntityType" });

            migrationBuilder.CreateIndex(
                name: "IX_Ecom_WebhookEvents_ProviderType_EventName_ProcessingStatus",
                table: "Ecom_WebhookEvents",
                columns: new[] { "ProviderType", "EventName", "ProcessingStatus" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ecom_OrderItems");

            migrationBuilder.DropTable(
                name: "Ecom_ProductMappings");

            migrationBuilder.DropTable(
                name: "Ecom_SyncQueue");

            migrationBuilder.DropTable(
                name: "Ecom_WebhookEvents");

            migrationBuilder.DropTable(
                name: "Ecom_Orders");

            migrationBuilder.DropTable(
                name: "Ecom_Stores");
        }
    }
}
