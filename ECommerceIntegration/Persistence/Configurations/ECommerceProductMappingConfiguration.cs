using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Persistence.Configurations;

public class ECommerceProductMappingConfiguration : IEntityTypeConfiguration<ECommerceProductMapping>
{
    public void Configure(EntityTypeBuilder<ECommerceProductMapping> builder)
    {
        builder.ToTable("Ecom_ProductMappings");
        builder.HasKey(x => x.Oid);

        builder.Property(x => x.ExternalProductId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ExternalVariantId).HasMaxLength(100);
        builder.Property(x => x.ExternalSku).HasMaxLength(100);
        builder.Property(x => x.ExternalBarcode).HasMaxLength(100);
        builder.Property(x => x.ExternalProductName).HasMaxLength(500);
        builder.Property(x => x.LocalBarcode).HasMaxLength(100);

        builder.HasIndex(x => new { x.StoreId, x.ExternalProductId, x.ExternalVariantId })
            .IsUnique(false);

        builder.HasIndex(x => new { x.StoreId, x.LocalProductId, x.LocalProductUnitId });

        builder.HasIndex(x => new { x.StoreId, x.IsMapped });

        builder.HasIndex(x => new { x.StoreId, x.ExternalBarcode });
    }
}
