using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Logs every RSD integration operation (AcceptBatch, PharmacySale, PharmacySaleCancel, ReturnBatch).
/// Master table — each call to SFDA RSD creates one row here, with product-level details in RsdOperationLogDetails.
/// </summary>
[Table("RsdOperationLogs")]
public class RsdOperationLog : BaseEntity
{
    /// <summary>
    /// FK to AppLookupDetail — RSD operation type (AcceptBatch, PharmacySale, PharmacySaleCancel, ReturnBatch)
    /// MasterID = 11111111-1111-1111-1111-111111111013
    /// </summary>
    public Guid? OperationTypeId { get; set; }

    [ForeignKey(nameof(OperationTypeId))]
    public virtual AppLookupDetail? OperationType { get; set; }

    /// <summary>
    /// Branch that initiated the operation
    /// </summary>
    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;

    /// <summary>
    /// GLN used in the request (FromGLN or ToGLN depending on operation)
    /// </summary>
    [MaxLength(20)]
    public string? GLN { get; set; }

    /// <summary>
    /// SFDA notification ID returned in the response
    /// </summary>
    [MaxLength(100)]
    public string? NotificationId { get; set; }

    /// <summary>
    /// Whether the SFDA call was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Response code from SFDA (OK, SOAP_FAULT, PARSE_ERROR, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? ResponseCode { get; set; }

    /// <summary>
    /// Response message / error description
    /// </summary>
    [MaxLength(2000)]
    public string? ResponseMessage { get; set; }

    /// <summary>
    /// Timestamp when the RSD call was made
    /// </summary>
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Full raw SOAP response for debugging
    /// </summary>
    public string? RawResponse { get; set; }

    /// <summary>
    /// Product-level details for this operation
    /// </summary>
    public virtual ICollection<RsdOperationLogDetail> Details { get; set; } = new List<RsdOperationLogDetail>();
}
