using AutoMapper;
using Pharmacy.Application.DTOs.Customer;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.IdentityTypeName,
                opt => opt.MapFrom(src => src.IdentityType != null ? src.IdentityType.ValueNameEn : null));

        CreateMap<CreateCustomerDto, Customer>();

        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.Oid,       opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt,  opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy,  opt => opt.Ignore())
            .ForMember(dest => dest.IsWalkIn,   opt => opt.Ignore());
    }
}
