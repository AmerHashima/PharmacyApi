using AutoMapper;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Application.Mappings;

public class InvoiceShapeProfile : Profile
{
    public InvoiceShapeProfile()
    {
        CreateMap<Domain.Entities.InvoiceShape, InvoiceShapeDto>()
            .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch.BranchName));

        CreateMap<CreateInvoiceShapeDto, Domain.Entities.InvoiceShape>();
    }
}
