using AutoMapper;
using Pharmacy.Application.DTOs.Customer;
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
            .ForMember(dest => dest.Customer,
                opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.DoctorFullNameEn,
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.FullNameEn : null))
            .ForMember(dest => dest.DoctorFullNameAr,
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.FullNameAr : null))
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
                opt => opt.MapFrom(src => src.Product != null ? src.Product.GTIN : null))
            .ForMember(dest => dest.OfferDetailId,
                opt => opt.MapFrom(src => src.OfferDetailId))
            .ForMember(dest => dest.OfferNameSnapshot,
                opt => opt.MapFrom(src => src.OfferNameSnapshot));
    }
}
