using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Persistence.Configurations;

public class ECommerceOrderItemConfiguration : IEntityTypeConfiguration<ECommerceOrderItem>
{
    public void Configure(EntityTypeBuilder<ECommerceOrderItem> builder)
    {
        builder.ToTable("Ecom_OrderItems");
        builder.HasKey(x => x.Oid);

        builder.Property(x => x.ExternalProductId).HasMaxLength(100);
        builder.Property(x => x.ExternalVariantId).HasMaxLength(100);
        builder.Property(x => x.ExternalSku).HasMaxLength(100);
        builder.Property(x => x.ExternalBarcode).HasMaxLength(100);
        builder.Property(x => x.ProductName).HasMaxLength(500).IsRequired();

        builder.Property(x => x.Quantity).HasColumnType("decimal(18,2)");
        builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.VatAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
    }
}
