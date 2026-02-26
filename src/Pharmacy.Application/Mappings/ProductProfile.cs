using AutoMapper;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for Product entity mappings
/// </summary>
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Entity to DTO
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductTypeName, 
                opt => opt.MapFrom(src => src.ProductType != null ? src.ProductType.ValueNameEn : null))
            .ForMember(dest => dest.ProductTypeNameAr, 
                opt => opt.MapFrom(src => src.ProductType != null ? src.ProductType.ValueNameAr : null))
            .ForMember(dest => dest.ProductGroupName, 
                opt => opt.MapFrom(src => src.ProductGroup != null ? src.ProductGroup.ValueNameEn : null))
            .ForMember(dest => dest.ProductGroupNameAr,
                opt => opt.MapFrom(src => src.ProductGroup != null ? src.ProductGroup.ValueNameAr : null))
            .ForMember(dest => dest.VatTypeName,
                opt => opt.MapFrom(src => src.VatType != null ? src.VatType.ValueNameEn : null))
            .ForMember(dest => dest.VatTypeNameAr,
                opt => opt.MapFrom(src => src.VatType != null ? src.VatType.ValueNameAr : null))
            ;

        // Create DTO to Entity
        CreateMap<CreateProductDto, Product>();

        // Update DTO to Entity
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
