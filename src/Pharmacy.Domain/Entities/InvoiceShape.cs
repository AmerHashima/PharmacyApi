using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents an invoice layout/shape tied to a branch.
/// </summary>
[Table("InvoiceShapes")]
public class InvoiceShape : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string ShapeName { get; set; } = string.Empty;

    /// <summary>
    /// HTML content for the invoice layout
    /// </summary>
    [Column(TypeName = "nvarchar(max)")]
    public string HtmlContent { get; set; } = string.Empty;

    /// <summary>
    /// Whether this shape should be used by default when printing
    /// </summary>
    public bool DefaultPrint { get; set; } = false;

    /// <summary>
    /// Whether this invoice shape is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// FK to Branch — the branch this invoice shape belongs to
    /// </summary>
    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;
}
