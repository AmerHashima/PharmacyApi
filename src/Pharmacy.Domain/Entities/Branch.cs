using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a pharmacy branch location.
/// A stakeholder (pharmacy) can have multiple branches.
/// </summary>
[Table("Branches")]
public class Branch : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string BranchCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Global Location Number - unique identifier for physical locations
    /// </summary>
    [MaxLength(20)]
    public string? GLN { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? District { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    // Navigation Properties
    public virtual ICollection<SystemUser> Users { get; set; } = new List<SystemUser>();
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public virtual ICollection<StockTransaction> OutgoingTransactions { get; set; } = new List<StockTransaction>();
    public virtual ICollection<StockTransaction> IncomingTransactions { get; set; } = new List<StockTransaction>();
    public virtual ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
}
