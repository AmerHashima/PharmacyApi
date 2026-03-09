namespace Pharmacy.Application.DTOs.Rsd;

public class ReturnBatchRequestDto
{
    public Guid BranchId { get; set; }

    /// <summary>
    /// Destination GLN (who you are returning to). If null, resolved from Branch GLN.
    /// </summary>
    public string? ToGLN { get; set; }

    public List<ReturnBatchProductItemDto> Products { get; set; } = new();
}

public class ReturnBatchProductItemDto
{
    public string GTIN { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
