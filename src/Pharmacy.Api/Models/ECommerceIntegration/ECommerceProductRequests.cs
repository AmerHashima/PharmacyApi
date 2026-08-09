namespace Pharmacy.Api.Models.ECommerceIntegration;

public class PullProviderProductsRequest
{
    public Guid StoreId { get; set; }
}

public class PushInitialProductsToProviderRequest
{
    public Guid StoreId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? WarehouseId { get; set; }
    public Guid? PriceListId { get; set; }
    public int BatchSize { get; set; } = 100;
}
