namespace Pharmacy.Application.DTOs.StockTransactionReturn;

/// <summary>
/// DTO for reading Stock Transaction Return data (master/header)
/// </summary>
public class StockTransactionReturnDto
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
    public Guid? ReturnInvoiceId { get; set; }
    public string? ReturnInvoiceNumber { get; set; }
    public Guid? OriginalTransactionId { get; set; }
    public string? Status { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedByName { get; set; }
}
