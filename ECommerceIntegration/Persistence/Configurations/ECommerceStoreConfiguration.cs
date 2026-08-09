using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Persistence.Configurations;

public class ECommerceStoreConfiguration : IEntityTypeConfiguration<ECommerceStore>
{
    public void Configure(EntityTypeBuilder<ECommerceStore> builder)
    {
        builder.ToTable("Ecom_Stores");
        builder.HasKey(x => x.Oid);

        builder.Property(x => x.ExternalStoreId).HasMaxLength(100);
        builder.Property(x => x.StoreName).HasMaxLength(250);
        builder.Property(x => x.AccessToken).HasMaxLength(4000);
        builder.Property(x => x.RefreshToken).HasMaxLength(4000);

        builder.HasIndex(x => new { x.ProviderType, x.ExternalStoreId })
            .IsUnique()
            .HasFilter("[ExternalStoreId] IS NOT NULL AND [IsDeleted] = 0");

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.Store)
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ProductMappings)
            .WithOne(x => x.Store)
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
