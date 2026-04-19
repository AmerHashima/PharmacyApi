using AutoMapper;
using Pharmacy.Application.DTOs.InvoiceSetup;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class InvoiceSetupProfile : Profile
{
    public InvoiceSetupProfile()
    {
        CreateMap<InvoiceSetup, InvoiceSetupDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null));

        CreateMap<CreateInvoiceSetupDto, InvoiceSetup>();

        CreateMap<UpdateInvoiceSetupDto, InvoiceSetup>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
