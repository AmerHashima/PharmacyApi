using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Detail row for an RSD operation log — one per product in the request/response.
/// </summary>
[Table("RsdOperationLogDetails")]
public class RsdOperationLogDetail : BaseEntity
{
    /// <summary>
    /// FK to parent RsdOperationLog
    /// </summary>
    [Required]
    public Guid RsdOperationLogId { get; set; }

    [ForeignKey(nameof(RsdOperationLogId))]
    public virtual RsdOperationLog RsdOperationLog { get; set; } = null!;

    /// <summary>
    /// GTIN sent in the request
    /// </summary>
    [MaxLength(50)]
    public string? GTIN { get; set; }

    /// <summary>
    /// Matched product from our Products table (nullable if not found)
    /// </summary>
    public Guid? ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }

    /// <summary>
    /// Batch number
    /// </summary>
    [MaxLength(100)]
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Expiry date as sent/received
    /// </summary>
    [MaxLength(20)]
    public string? ExpiryDate { get; set; }

    /// <summary>
    /// Quantity (for batch operations)
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Serial number (for PharmacySale operations)
    /// </summary>
    [MaxLength(100)]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Per-product response code from SFDA (e.g. 10201)
    /// </summary>
    [MaxLength(20)]
    public string? ResponseCode { get; set; }
}
