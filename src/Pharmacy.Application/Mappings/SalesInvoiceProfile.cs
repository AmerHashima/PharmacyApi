using AutoMapper;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for SalesInvoice entity mappings
/// </summary>
public class SalesInvoiceProfile : Profile
{
    public SalesInvoiceProfile()
    {
        // SalesInvoice Entity to DTO
        CreateMap<SalesInvoice, SalesInvoiceDto>()
            .ForMember(dest => dest.BranchName, 
                opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : string.Empty))
            .ForMember(dest => dest.PaymentMethodName, 
                opt => opt.MapFrom(src => src.PaymentMethod != null ? src.PaymentMethod.ValueNameEn : null))
            .ForMember(dest => dest.InvoiceStatusName, 
                opt => opt.MapFrom(src => src.InvoiceStatus != null ? src.InvoiceStatus.ValueNameEn : null))
            .ForMember(dest => dest.CashierName, 
                opt => opt.MapFrom(src => src.Cashier != null ? src.Cashier.FullName : null))
            .ForMember(dest => dest.Items, 
                opt => opt.MapFrom(src => src.Items));

        // SalesInvoiceItem Entity to DTO
        CreateMap<SalesInvoiceItem, SalesInvoiceItemDto>()
            .ForMember(dest => dest.ProductName, 
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : string.Empty))
            .ForMember(dest => dest.ProductGTIN, 
                opt => opt.MapFrom(src => src.Product != null ? src.Product.GTIN : null));
    }
}
