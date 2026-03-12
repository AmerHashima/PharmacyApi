using AutoMapper;
using Pharmacy.Application.DTOs.ProductUnit;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class ProductUnitProfile : Profile
{
    public ProductUnitProfile()
    {
        CreateMap<ProductUnit, ProductUnitDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : null))
            .ForMember(dest => dest.PackageTypeName,
                opt => opt.MapFrom(src => src.PackageType != null ? src.PackageType.ValueNameEn : null));

        CreateMap<CreateProductUnitDto, ProductUnit>();

        CreateMap<UpdateProductUnitDto, ProductUnit>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
