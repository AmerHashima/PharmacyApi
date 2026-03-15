using AutoMapper;
using Pharmacy.Application.DTOs.ReturnInvoice;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for ReturnInvoice entity mappings
/// </summary>
public class ReturnInvoiceProfile : Profile
{
    public ReturnInvoiceProfile()
    {
        // ReturnInvoice Entity to DTO
        CreateMap<ReturnInvoice, ReturnInvoiceDto>()
            .ForMember(dest => dest.OriginalInvoiceNumber,
                opt => opt.MapFrom(src => src.OriginalInvoice != null ? src.OriginalInvoice.InvoiceNumber : null))
            .ForMember(dest => dest.BranchName,
                opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : string.Empty))
            .ForMember(dest => dest.PaymentMethodName,
                opt => opt.MapFrom(src => src.PaymentMethod != null ? src.PaymentMethod.ValueNameEn : null))
            .ForMember(dest => dest.InvoiceStatusName,
                opt => opt.MapFrom(src => src.InvoiceStatus != null ? src.InvoiceStatus.ValueNameEn : null))
            .ForMember(dest => dest.CashierName,
                opt => opt.MapFrom(src => src.Cashier != null ? src.Cashier.FullName : null))
            .ForMember(dest => dest.ReturnReasonName,
                opt => opt.MapFrom(src => src.ReturnReason != null ? src.ReturnReason.ValueNameEn : null))
            .ForMember(dest => dest.Items,
                opt => opt.MapFrom(src => src.Items));

        // ReturnInvoiceItem Entity to DTO
        CreateMap<ReturnInvoiceItem, ReturnInvoiceItemDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : string.Empty))
            .ForMember(dest => dest.ProductGTIN,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.GTIN : null));
    }
}
