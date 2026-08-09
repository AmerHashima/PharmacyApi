namespace ECommerceIntegration.Application.Dtos;

public class LocalProductMatchDto
{
    public Guid LocalProductId { get; set; }
    public Guid? LocalProductUnitId { get; set; }
    public string? LocalBarcode { get; set; }
}
