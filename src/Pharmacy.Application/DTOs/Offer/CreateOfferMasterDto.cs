using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Offer;

public class CreateOfferMasterDto
{
    [Required]
    [MaxLength(300)]
    public string OfferNameAr { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string OfferNameEn { get; set; } = string.Empty;

    [Required]
    public Guid OfferTypeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? BranchId { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public List<CreateOfferDetailDto> OfferDetails { get; set; } = new();
}
