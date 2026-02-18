namespace Pharmacy.Application.DTOs.StockTransaction;

/// <summary>
/// DTO for reading Stock Transaction data
/// </summary>
public class StockTransactionDto
{
    public Guid Oid { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductGTIN { get; set; }
    public Guid? FromBranchId { get; set; }
    public string? FromBranchName { get; set; }
    public Guid? ToBranchId { get; set; }
    public string? ToBranchName { get; set; }
    public decimal Quantity { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public string? TransactionTypeName { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? TransactionDate { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? TotalValue { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedByName { get; set; }
}
