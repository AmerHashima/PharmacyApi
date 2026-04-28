using AutoMapper;
using Pharmacy.Application.DTOs.Offer;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class OfferProfile : Profile
{
    public OfferProfile()
    {
        // ── OfferMaster ────────────────────────────────────────────
        CreateMap<OfferMaster, OfferMasterDto>()
            .ForMember(dest => dest.OfferTypeNameEn,
                opt => opt.MapFrom(src => src.OfferType != null ? src.OfferType.ValueNameEn : null))
            .ForMember(dest => dest.OfferTypeNameAr,
                opt => opt.MapFrom(src => src.OfferType != null ? src.OfferType.ValueNameAr : null))
            .ForMember(dest => dest.BranchName,
                opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null));

        CreateMap<CreateOfferMasterDto, OfferMaster>()
            .ForMember(dest => dest.OfferDetails, opt => opt.Ignore());

        CreateMap<UpdateOfferMasterDto, OfferMaster>()
            .ForMember(dest => dest.Oid,          opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt,     opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy,     opt => opt.Ignore())
            .ForMember(dest => dest.OfferDetails,  opt => opt.Ignore());

        // ── OfferDetail ────────────────────────────────────────────
        CreateMap<OfferDetail, OfferDetailDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : null))
            .ForMember(dest => dest.ProductNameAr,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugNameAr : null))
            .ForMember(dest => dest.ProductBarcode,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Barcode : null))
            .ForMember(dest => dest.FreeProductName,
                opt => opt.MapFrom(src => src.FreeProduct != null ? src.FreeProduct.DrugName : null))
            .ForMember(dest => dest.FreeProductNameAr,
                opt => opt.MapFrom(src => src.FreeProduct != null ? src.FreeProduct.DrugNameAr : null));

        CreateMap<CreateOfferDetailDto, OfferDetail>();

        CreateMap<UpdateOfferDetailDto, OfferDetail>()
            .ForMember(dest => dest.Oid,       opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
