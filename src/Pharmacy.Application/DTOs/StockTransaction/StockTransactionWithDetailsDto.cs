namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for stock transaction with details (complete view)
/// </summary>
public class StockTransactionWithDetailsDto
{
    public Guid Oid { get; set; }
    public Guid TransactionTypeId { get; set; }
    public string? TransactionTypeName { get; set; }
    public Guid? FromBranchId { get; set; }
    public string? FromBranchName { get; set; }
    public Guid? ToBranchId { get; set; }
    public string? ToBranchName { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? NotificationId { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal TotalValue { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = "Draft";
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Detail lines
    /// </summary>
    public List<StockTransactionDetailDto> Details { get; set; } = new();
}
