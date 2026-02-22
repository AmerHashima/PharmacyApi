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
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.DrugName))
                .ForMember(dest => dest.ProductGTIN, opt => opt.MapFrom(src => src.Product.GTIN))

            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName));

        // Create DTO to Entity
        CreateMap<CreateStockDto, Stock>();
    }
}
