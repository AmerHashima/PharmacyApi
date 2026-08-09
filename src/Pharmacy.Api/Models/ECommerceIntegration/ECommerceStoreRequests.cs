using ECommerceIntegration.Domain.Enums;

namespace Pharmacy.Api.Models.ECommerceIntegration;

public class CreateECommerceStoreRequest
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
}

public class ConnectECommerceStoreRequest
{
    public ECommerceProviderType ProviderType { get; set; }
    public string Code { get; set; } = null!;
    public Guid? BranchId { get; set; }
    public Guid? WarehouseId { get; set; }
    public Guid? PriceListId { get; set; }
}

public class UpdateECommerceStoreSettingsRequest
{
    public Guid? BranchId { get; set; }
    public Guid? WarehouseId { get; set; }
    public Guid? PriceListId { get; set; }
    public bool AutoPullOrders { get; set; } = true;
    public bool AutoSyncStock { get; set; } = true;
    public bool AutoSyncPrice { get; set; }
    public bool IsActive { get; set; } = true;
}
