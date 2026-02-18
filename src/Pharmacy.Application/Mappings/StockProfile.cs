using AutoMapper;
using Pharmacy.Application.DTOs.Stock;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for Stock entity mappings
/// </summary>
public class StockProfile : Profile
{
    public StockProfile()
    {
        // Entity to DTO
        CreateMap<Stock, StockDto>()
            .ForMember(dest => dest.ProductName, 
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : string.Empty))
            .ForMember(dest => dest.ProductGTIN, 
                opt => opt.MapFrom(src => src.Product != null ? src.Product.GTIN : null))
            .ForMember(dest => dest.BranchName, 
                opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : string.Empty));

        // Create DTO to Entity
        CreateMap<CreateStockDto, Stock>();
    }
}
