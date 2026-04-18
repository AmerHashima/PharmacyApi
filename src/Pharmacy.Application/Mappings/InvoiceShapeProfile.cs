using AutoMapper;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Application.Mappings;

public class InvoiceShapeProfile : Profile
{
    public InvoiceShapeProfile()
    {
        // Entity → DTO
        CreateMap<Domain.Entities.InvoiceShape, InvoiceShapeDto>()
            .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null));

        // CreateDto → Entity
        CreateMap<CreateInvoiceShapeDto, Domain.Entities.InvoiceShape>()
            .ForMember(d => d.Oid, opt => opt.Ignore());

        // UpdateDto → Entity
        CreateMap<UpdateInvoiceShapeDto, Domain.Entities.InvoiceShape>()
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore());
    }
}
