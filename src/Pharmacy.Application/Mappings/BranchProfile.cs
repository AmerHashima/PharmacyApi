using AutoMapper;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for Branch entity mappings
/// </summary>
public class BranchProfile : Profile
{
    public BranchProfile()
    {
        // Entity to DTO
        CreateMap<Branch, BranchDto>()
           
            .ForMember(dest => dest.IdentifyLookupName, opt => opt.MapFrom(src => src.IdentifyLookup != null ? src.IdentifyLookup.ValueNameEn : null));

        // Create DTO to Entity
        CreateMap<CreateBranchDto, Branch>();

        // Update DTO to Entity
        CreateMap<UpdateBranchDto, Branch>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
