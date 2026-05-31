using AutoMapper;
using Pharmacy.Application.DTOs.SalesInvoicePayment;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class SalesInvoicePaymentProfile : Profile
{
    public SalesInvoicePaymentProfile()
    {
        CreateMap<Domain.Entities.SalesInvoicePayment, SalesInvoicePaymentDto>()
            .ForMember(d => d.InvoiceNumber,
                o => o.MapFrom(s => s.SalesInvoice != null ? s.SalesInvoice.InvoiceNumber : null))
            .ForMember(d => d.ShiftNumber,
                o => o.MapFrom(s => s.Shift != null ? s.Shift.ShiftNumber : null))
            .ForMember(d => d.PaymentMethodName,
                o => o.MapFrom(s => s.PaymentMethod != null ? s.PaymentMethod.ValueNameEn : null));

        CreateMap<CreateSalesInvoicePaymentDto, Domain.Entities.SalesInvoicePayment>()
            .ForMember(d => d.Oid, o => o.Ignore());
    }
}
