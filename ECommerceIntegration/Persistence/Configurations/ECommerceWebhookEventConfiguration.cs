using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Persistence.Configurations;

public class ECommerceWebhookEventConfiguration : IEntityTypeConfiguration<ECommerceWebhookEvent>
{
    public void Configure(EntityTypeBuilder<ECommerceWebhookEvent> builder)
    {
        builder.ToTable("Ecom_WebhookEvents");
        builder.HasKey(x => x.Oid);

        builder.Property(x => x.EventName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.ExternalId).HasMaxLength(100);
        builder.Property(x => x.Payload).IsRequired();
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasIndex(x => new { x.ProviderType, x.EventName, x.ProcessingStatus });
    }
}
