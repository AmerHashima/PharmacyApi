using AutoMapper;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for StockTransactionDetail entity mappings
/// </summary>
public class StockTransactionDetailProfile : Profile
{
    public StockTransactionDetailProfile()
    {
        // Entity to DTO
        CreateMap<StockTransactionDetail, StockTransactionDetailDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : null))
            .ForMember(dest => dest.ProductGTIN,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.GTIN : null));

        // Create DTO to Entity
        CreateMap<CreateStockTransactionDetailDto, StockTransactionDetail>();
        CreateMap<CreateStockTransactionDetailForMasterDto, StockTransactionDetail>();

        // Update DTO to Entity
        CreateMap<UpdateStockTransactionDetailDto, StockTransactionDetail>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
