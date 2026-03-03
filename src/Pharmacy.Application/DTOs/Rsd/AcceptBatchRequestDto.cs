
namespace Pharmacy.Application.DTOs.Rsd;

public class AcceptBatchRequestDto
{
    public Guid BranchId { get; set; }

    /// <summary>
    /// Sender GLN — if null, resolved from Branch settings
    /// </summary>
    public string? FromGLN { get; set; }

    public List<AcceptBatchProductItemDto> Products { get; set; } = new();
}

public class AcceptBatchProductItemDto
{
    public string GTIN { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
}