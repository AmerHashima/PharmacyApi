using AutoMapper;
using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for StockTransactionReturn entity mappings
/// </summary>
public class StockTransactionReturnProfile : Profile
{
    public StockTransactionReturnProfile()
    {
        // Entity to DTO (list view)
        CreateMap<StockTransactionReturn, StockTransactionReturnDto>()
            .ForMember(dest => dest.FromBranchName,
                opt => opt.MapFrom(src => src.FromBranch != null ? src.FromBranch.BranchName : null))
            .ForMember(dest => dest.ToBranchName,
                opt => opt.MapFrom(src => src.ToBranch != null ? src.ToBranch.BranchName : null))
            .ForMember(dest => dest.TransactionTypeName,
                opt => opt.MapFrom(src => src.TransactionType != null ? src.TransactionType.ValueNameEn : null))
            .ForMember(dest => dest.SupplierName,
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
            .ForMember(dest => dest.ReturnInvoiceNumber,
                opt => opt.MapFrom(src => src.ReturnInvoice != null ? src.ReturnInvoice.ReturnNumber : null));

        // Entity to WithDetailsDto (detail view)
        CreateMap<StockTransactionReturn, StockTransactionReturnWithDetailsDto>()
            .ForMember(dest => dest.FromBranchName,
                opt => opt.MapFrom(src => src.FromBranch != null ? src.FromBranch.BranchName : null))
            .ForMember(dest => dest.ToBranchName,
                opt => opt.MapFrom(src => src.ToBranch != null ? src.ToBranch.BranchName : null))
            .ForMember(dest => dest.TransactionTypeName,
                opt => opt.MapFrom(src => src.TransactionType != null ? src.TransactionType.ValueNameEn : null))
            .ForMember(dest => dest.SupplierName,
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
            .ForMember(dest => dest.ReturnInvoiceNumber,
                opt => opt.MapFrom(src => src.ReturnInvoice != null ? src.ReturnInvoice.ReturnNumber : null))
            .ForMember(dest => dest.Details,
                opt => opt.MapFrom(src => src.Details));

        // Detail Entity to DetailDto
        CreateMap<StockTransactionReturnDetail, StockTransactionReturnDetailDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.TradeName : null))
            .ForMember(dest => dest.ProductGTIN,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.GTIN : null));

        // Create DTO to Entity
        CreateMap<CreateStockTransactionReturnWithDetailsDto, StockTransactionReturn>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.Details, opt => opt.Ignore());
    }
}
