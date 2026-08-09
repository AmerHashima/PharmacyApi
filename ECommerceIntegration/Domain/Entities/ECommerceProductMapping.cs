using ECommerceIntegration.Domain.Common;

namespace ECommerceIntegration.Domain.Entities;

public class ECommerceProductMapping : ECommerceBaseEntity
{
    public Guid StoreId { get; set; }
    public ECommerceStore Store { get; set; } = null!;
    public string ExternalProductId { get; set; } = null!;
    public string? ExternalVariantId { get; set; }
    public string? ExternalSku { get; set; }
    public string? ExternalBarcode { get; set; }
    public string? ExternalProductName { get; set; }
    public Guid? LocalProductId { get; set; }
    public Guid? LocalProductUnitId { get; set; }
    public string? LocalBarcode { get; set; }
    public bool IsMapped { get; set; }
    public bool IsActive { get; set; } = true;
}
