using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Persistence.Configurations;

public class ECommerceSyncQueueConfiguration : IEntityTypeConfiguration<ECommerceSyncQueue>
{
    public void Configure(EntityTypeBuilder<ECommerceSyncQueue> builder)
    {
        builder.ToTable("Ecom_SyncQueue");
        builder.HasKey(x => x.Oid);

        builder.Property(x => x.EntityType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ActionType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ExternalEntityId).HasMaxLength(100);
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasIndex(x => new { x.Status, x.ProviderType, x.EntityType });
    }
}
