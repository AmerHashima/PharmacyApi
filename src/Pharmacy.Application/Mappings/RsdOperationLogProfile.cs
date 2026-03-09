using AutoMapper;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class RsdOperationLogProfile : Profile
{
    public RsdOperationLogProfile()
    {
        CreateMap<RsdOperationLog, RsdOperationLogDto>()
            .ForMember(dest => dest.OperationTypeName,
                opt => opt.MapFrom(src => src.OperationType != null ? src.OperationType.ValueNameEn : null))
            .ForMember(dest => dest.BranchName,
                opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null));

        CreateMap<RsdOperationLog, RsdOperationLogWithDetailsDto>()
            .ForMember(dest => dest.OperationTypeName,
                opt => opt.MapFrom(src => src.OperationType != null ? src.OperationType.ValueNameEn : null))
            .ForMember(dest => dest.BranchName,
                opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null));

        CreateMap<RsdOperationLogDetail, RsdOperationLogDetailDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.DrugName : null));
    }
}
