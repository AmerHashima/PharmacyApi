namespace Pharmacy.Api.Models.ECommerceIntegration;

public class PullOrdersRequest
{
    public Guid StoreId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class PullOrderRequest
{
    public Guid StoreId { get; set; }
}
