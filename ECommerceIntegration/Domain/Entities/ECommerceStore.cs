using ECommerceIntegration.Domain.Common;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Domain.Entities;

public class ECommerceStore : ECommerceBaseEntity
{
    public ECommerceProviderType ProviderType { get; set; }
    public string? ExternalStoreId { get; set; }
    public string? StoreName { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiresAt { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? WarehouseId { get; set; }
    public Guid? PriceListId { get; set; }
    public bool AutoPullOrders { get; set; } = true;
    public bool AutoSyncStock { get; set; } = true;
    public bool AutoSyncPrice { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<ECommerceOrder> Orders { get; set; } = new List<ECommerceOrder>();
    public ICollection<ECommerceProductMapping> ProductMappings { get; set; } = new List<ECommerceProductMapping>();
}
