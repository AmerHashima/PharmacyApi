using AutoMapper;
using Pharmacy.Application.DTOs.Link;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class LinkProfile : Profile
{
    public LinkProfile()
    {
        // ── Link ──────────────────────────────────────────────────
        CreateMap<Link, LinkDto>();

        CreateMap<CreateLinkDto, Link>()
            .ForMember(dest => dest.ReportParameters, opt => opt.Ignore());

        CreateMap<UpdateLinkDto, Link>()
            .ForMember(dest => dest.Oid,               opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt,          opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy,          opt => opt.Ignore())
            .ForMember(dest => dest.ReportParameters,   opt => opt.Ignore());

        // ── ReportParameter ───────────────────────────────────────
        CreateMap<ReportParameter, ReportParameterDto>();

        CreateMap<CreateReportParameterDto, ReportParameter>()
            .ForMember(dest => dest.LinksOid, opt => opt.Ignore());
    }
}
