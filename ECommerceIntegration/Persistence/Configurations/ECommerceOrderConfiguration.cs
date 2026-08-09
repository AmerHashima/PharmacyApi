using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Persistence.Configurations;

public class ECommerceOrderConfiguration : IEntityTypeConfiguration<ECommerceOrder>
{
    public void Configure(EntityTypeBuilder<ECommerceOrder> builder)
    {
        builder.ToTable("Ecom_Orders");
        builder.HasKey(x => x.Oid);

        builder.Property(x => x.ExternalOrderId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ExternalOrderNumber).HasMaxLength(100);
        builder.Property(x => x.CustomerName).HasMaxLength(250);
        builder.Property(x => x.CustomerMobile).HasMaxLength(50);
        builder.Property(x => x.CustomerEmail).HasMaxLength(250);
        builder.Property(x => x.OrderStatus).HasMaxLength(100);
        builder.Property(x => x.PaymentStatus).HasMaxLength(100);
        builder.Property(x => x.PaymentMethod).HasMaxLength(100);
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.ShippingAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.VatAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");

        builder.HasIndex(x => new { x.ProviderType, x.ExternalOrderId })
            .IsUnique();

        builder.HasMany(x => x.Items)
            .WithOne(x => x.ECommerceOrder)
            .HasForeignKey(x => x.ECommerceOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
