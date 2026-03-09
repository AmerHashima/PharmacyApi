namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// DTO for listing RSD Operation Logs (without details — used in query/pagination)
/// </summary>
public class RsdOperationLogDto
{
    public Guid Oid { get; set; }
    public Guid? OperationTypeId { get; set; }
    public string? OperationTypeName { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? GLN { get; set; }
    public string? NotificationId { get; set; }
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}

/// <summary>
/// DTO for reading a single RSD Operation Log with its detail lines (used in get-by-id)
/// </summary>
public class RsdOperationLogWithDetailsDto
{
    public Guid Oid { get; set; }
    public Guid? OperationTypeId { get; set; }
    public string? OperationTypeName { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? GLN { get; set; }
    public string? NotificationId { get; set; }
    public bool Success { get; set; }
    public string? ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    public List<RsdOperationLogDetailDto> Details { get; set; } = new();
}

/// <summary>
/// DTO for reading RSD Operation Log Detail data
/// </summary>
public class RsdOperationLogDetailDto
{
    public Guid Oid { get; set; }
    public string? GTIN { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? BatchNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public string? SerialNumber { get; set; }
    public string? ResponseCode { get; set; }
}
