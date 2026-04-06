namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for reading Stock Transaction data (master/header)
/// </summary>
public class StockTransactionDto
{
    public Guid Oid { get; set; }
    public Guid? FromBranchId { get; set; }
    public string? FromBranchName { get; set; }
    public Guid? ToBranchId { get; set; }
    public string? ToBranchName { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public string? TransactionTypeName { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? TransactionDate { get; set; }
    public decimal? TotalValue { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedByName { get; set; }

    /// <summary>
    /// Detail lines for this transaction
    /// </summary>
   // public List<StockTransactionDetailDto> Details { get; set; } = new();
}
