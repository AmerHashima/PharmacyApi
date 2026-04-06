using AutoMapper;
using Pharmacy.Application.DTOs.Store;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for Store entity mappings
/// </summary>
public class StoreProfile : Profile
{
    public StoreProfile()
    {
        // Entity → DTO
        CreateMap<Store, StoreDto>()
            .ForMember(dest => dest.BranchName,
                opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null));

        // CreateDto → Entity
        CreateMap<CreateStoreDto, Store>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore());

        // UpdateDto → Entity
        CreateMap<UpdateStoreDto, Store>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
