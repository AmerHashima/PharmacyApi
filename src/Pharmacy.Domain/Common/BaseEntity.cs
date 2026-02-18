using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public Guid Oid { get; set; } = Guid.NewGuid();

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
