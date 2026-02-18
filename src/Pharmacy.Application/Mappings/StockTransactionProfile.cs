using AutoMapper;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for StockTransaction entity mappings
/// </summary>
public class StockTransactionProfile : Profile
{
    public StockTransactionProfile()
    {
        // Entity to DTO
        CreateMap<StockTransaction, StockTransactionDto>()
            .ForMember(dest => dest.ProductName, 
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : string.Empty))
            .ForMember(dest => dest.ProductGTIN, 
                opt => opt.MapFrom(src => src.Product != null ? src.Product.GTIN : null))
            .ForMember(dest => dest.FromBranchName, 
                opt => opt.MapFrom(src => src.FromBranch != null ? src.FromBranch.BranchName : null))
            .ForMember(dest => dest.ToBranchName, 
                opt => opt.MapFrom(src => src.ToBranch != null ? src.ToBranch.BranchName : null))
            .ForMember(dest => dest.TransactionTypeName, 
                opt => opt.MapFrom(src => src.TransactionType != null ? src.TransactionType.ValueNameEn : null))
            .ForMember(dest => dest.SupplierName, 
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null));
    }
}
