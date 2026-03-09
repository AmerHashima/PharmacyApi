namespace Pharmacy.Application.DTOs.Rsd;

public class DispatchDetailResponseDto
{
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public string? DispatchNotificationId { get; set; }
    public string? NotificationDate { get; set; }
    public string? FromGLN { get; set; }

    /// <summary>Supplier matched by FromGLN from Stakeholders table</summary>
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }

    public List<DispatchProductDto> Products { get; set; } = new();
    public string? RawResponse { get; set; }
}

public class DispatchProductDto
{
    /// <summary>Global Trade Item Number</summary>
    public string GTIN { get; set; } = string.Empty;

    /// <summary>Batch Number</summary>
    public string? BatchNumber { get; set; }

    /// <summary>Expiry Date</summary>
    public string? ExpiryDate { get; set; }

    /// <summary>Quantity</summary>
    public int Quantity { get; set; }

    /// <summary>Response Code (00000 = success)</summary>
    public string? ResponseCode { get; set; }

    /// <summary>Product matched by GTIN from Products table</summary>
    public Guid? ProductId { get; set; }
    public string? ProductName { get; set; }
}