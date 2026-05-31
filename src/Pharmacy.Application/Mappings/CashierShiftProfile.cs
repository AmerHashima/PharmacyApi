using AutoMapper;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class CashierShiftProfile : Profile
{
    public CashierShiftProfile()
    {
        CreateMap<Domain.Entities.CashierShift, CashierShiftDto>()
            .ForMember(d => d.BranchName,
                o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.CashBoxName,
                o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameEn : null))
            .ForMember(d => d.UserName,
                o => o.MapFrom(s => s.User != null ? s.User.Username : null));

        CreateMap<Domain.Entities.CashierShift, CashierShiftWithDetailsDto>()
            .ForMember(d => d.BranchName,
                o => o.MapFrom(s => s.Branch != null ? s.Branch.BranchName : null))
            .ForMember(d => d.CashBoxName,
                o => o.MapFrom(s => s.CashBox != null ? s.CashBox.NameEn : null))
            .ForMember(d => d.UserName,
                o => o.MapFrom(s => s.User != null ? s.User.Username : null))
            .ForMember(d => d.Details,
                o => o.MapFrom(s => s.Details));

        CreateMap<CashierShiftDetail, CashierShiftDetailDto>()
            .ForMember(d => d.TransactionTypeName,
                o => o.MapFrom(s => s.TransactionType != null ? s.TransactionType.ValueNameEn : null))
            .ForMember(d => d.PaymentMethodName,
                o => o.MapFrom(s => s.PaymentMethod != null ? s.PaymentMethod.ValueNameEn : null));

        CreateMap<OpenCashierShiftDto, Domain.Entities.CashierShift>()
            .ForMember(d => d.Oid, o => o.Ignore())
            .ForMember(d => d.ShiftNumber, o => o.Ignore())
            .ForMember(d => d.Details, o => o.Ignore());

        CreateMap<AddCashierShiftDetailDto, CashierShiftDetail>()
            .ForMember(d => d.Oid, o => o.Ignore());
    }
}
