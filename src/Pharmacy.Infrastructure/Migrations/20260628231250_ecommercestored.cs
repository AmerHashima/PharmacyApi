using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ecommercestored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[Ecom_ProductMappings]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[Ecom_ProductMappings]', N'LocalProductUnitId') IS NULL
                BEGIN
                    ALTER TABLE [Ecom_ProductMappings]
                    ADD [LocalProductUnitId] uniqueidentifier NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[Ecom_ProductMappings]', N'U') IS NOT NULL
                   AND NOT EXISTS (
                       SELECT 1
                       FROM sys.indexes
                       WHERE [name] = N'IX_Ecom_ProductMappings_StoreId_IsMapped'
                         AND [object_id] = OBJECT_ID(N'[Ecom_ProductMappings]', N'U')
                   )
                BEGIN
                    CREATE INDEX [IX_Ecom_ProductMappings_StoreId_IsMapped]
                    ON [Ecom_ProductMappings] ([StoreId], [IsMapped]);
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[Ecom_ProductMappings]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[Ecom_ProductMappings]', N'LocalProductUnitId') IS NOT NULL
                   AND NOT EXISTS (
                       SELECT 1
                       FROM sys.indexes
                       WHERE [name] = N'IX_Ecom_ProductMappings_StoreId_LocalProductId_LocalProductUnitId'
                         AND [object_id] = OBJECT_ID(N'[Ecom_ProductMappings]', N'U')
                   )
                BEGIN
                    CREATE INDEX [IX_Ecom_ProductMappings_StoreId_LocalProductId_LocalProductUnitId]
                    ON [Ecom_ProductMappings] ([StoreId], [LocalProductId], [LocalProductUnitId]);
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[Ecom_ProductMappings]', N'U') IS NOT NULL
                   AND EXISTS (
                       SELECT 1
                       FROM sys.indexes
                       WHERE [name] = N'IX_Ecom_ProductMappings_StoreId_LocalProductId_LocalProductUnitId'
                         AND [object_id] = OBJECT_ID(N'[Ecom_ProductMappings]', N'U')
                   )
                BEGIN
                    DROP INDEX [IX_Ecom_ProductMappings_StoreId_LocalProductId_LocalProductUnitId]
                    ON [Ecom_ProductMappings];
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[Ecom_ProductMappings]', N'U') IS NOT NULL
                   AND EXISTS (
                       SELECT 1
                       FROM sys.indexes
                       WHERE [name] = N'IX_Ecom_ProductMappings_StoreId_IsMapped'
                         AND [object_id] = OBJECT_ID(N'[Ecom_ProductMappings]', N'U')
                   )
                BEGIN
                    DROP INDEX [IX_Ecom_ProductMappings_StoreId_IsMapped]
                    ON [Ecom_ProductMappings];
                END
                """);

            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[Ecom_ProductMappings]', N'U') IS NOT NULL
                   AND COL_LENGTH(N'[Ecom_ProductMappings]', N'LocalProductUnitId') IS NOT NULL
                BEGIN
                    ALTER TABLE [Ecom_ProductMappings]
                    DROP COLUMN [LocalProductUnitId];
                END
                """);
        }
    }
}
