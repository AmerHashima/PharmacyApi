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
        CreateMap<Domain.Entities.StockTransaction, StockTransactionDto>()
            .ForMember(dest => dest.FromBranchName, 
                opt => opt.MapFrom(src => src.FromBranch != null ? src.FromBranch.BranchName : null))
            .ForMember(dest => dest.ToBranchName, 
                opt => opt.MapFrom(src => src.ToBranch != null ? src.ToBranch.BranchName : null))
            .ForMember(dest => dest.TransactionTypeName, 
                opt => opt.MapFrom(src => src.TransactionType != null ? src.TransactionType.ValueNameEn : null))
            .ForMember(dest => dest.SupplierName, 
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
            .ForMember(dest => dest.Details,
                opt => opt.MapFrom(src => src.Details));

        // Entity to StockTransactionWithDetailsDto
        CreateMap<Domain.Entities.StockTransaction, StockTransactionWithDetailsDto>()
            .ForMember(dest => dest.FromBranchName, 
                opt => opt.MapFrom(src => src.FromBranch != null ? src.FromBranch.BranchName : null))
            .ForMember(dest => dest.ToBranchName, 
                opt => opt.MapFrom(src => src.ToBranch != null ? src.ToBranch.BranchName : null))
            .ForMember(dest => dest.TransactionTypeName, 
                opt => opt.MapFrom(src => src.TransactionType != null ? src.TransactionType.ValueNameEn : null))
            .ForMember(dest => dest.SupplierName, 
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
            .ForMember(dest => dest.Details,
                opt => opt.MapFrom(src => src.Details));

        // Create DTO to Entity
        CreateMap<CreateStockTransactionWithDetailsDto, Domain.Entities.StockTransaction>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.Details, opt => opt.Ignore());

        // Update DTO to Entity
        CreateMap<UpdateStockTransactionWithDetailsDto, Domain.Entities.StockTransaction>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Details, opt => opt.Ignore());
    }
}
