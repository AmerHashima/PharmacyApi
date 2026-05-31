using AutoMapper;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class PurchaseInvoiceProfile : Profile
{
    public PurchaseInvoiceProfile()
    {
        CreateMap<Domain.Entities.PurchaseInvoice, PurchaseInvoiceDto>()
            .ForMember(d => d.BranchName,
                o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.SupplierName,
                o => o.MapFrom(s => s.Supplier != null ? s.Supplier.Name : null))
            .ForMember(d => d.InvoiceStatusName,
                o => o.MapFrom(s => s.InvoiceStatus != null ? s.InvoiceStatus.ValueNameEn : null));

        CreateMap<CreatePurchaseInvoiceDto, Domain.Entities.PurchaseInvoice>()
            .ForMember(d => d.Oid, o => o.Ignore())
            .ForMember(d => d.PurchaseInvoiceNumber, o => o.Ignore())
            .ForMember(d => d.Payments, o => o.Ignore());

        CreateMap<UpdatePurchaseInvoiceDto, Domain.Entities.PurchaseInvoice>()
            .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<PurchaseInvoicePayment, PurchaseInvoicePaymentDto>()
            .ForMember(d => d.PaymentMethodName,
                o => o.MapFrom(s => s.PaymentMethod != null ? s.PaymentMethod.ValueNameEn : null));

        CreateMap<CreatePurchaseInvoicePaymentDto, PurchaseInvoicePayment>()
            .ForMember(d => d.Oid, o => o.Ignore())
            .ForMember(d => d.PurchaseInvoiceId, o => o.Ignore());
    }
}
