namespace Pharmacy.Application.DTOs.Offer;

public class OfferMasterDto
{
    public Guid Oid { get; set; }
    public string OfferNameAr { get; set; } = string.Empty;
    public string OfferNameEn { get; set; } = string.Empty;

    public Guid OfferTypeId { get; set; }
    public string? OfferTypeNameEn { get; set; }
    public string? OfferTypeNameAr { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }

    public string? Notes { get; set; }

    public List<OfferDetailDto> OfferDetails { get; set; } = new();
}
