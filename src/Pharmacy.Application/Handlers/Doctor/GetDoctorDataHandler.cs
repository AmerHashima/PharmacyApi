using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Doctor;
using Pharmacy.Application.Queries.Doctor;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Doctor;

public class GetDoctorDataHandler : IRequestHandler<GetDoctorDataQuery, PagedResult<DoctorDto>>
{
    private readonly IDoctorRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetDoctorDataHandler(IDoctorRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<DoctorDto>> Handle(GetDoctorDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(d => d.Specialty)
            .Include(d => d.ReferralType)
            .Include(d => d.IdentityType)
            .Where(d => !d.IsDeleted);

        var pagedEntities = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        var mappedData = _mapper.Map<IEnumerable<DoctorDto>>(pagedEntities.Data);

        return new PagedResult<DoctorDto>
        {
            Data = mappedData,
            TotalRecords = pagedEntities.TotalRecords,
            PageNumber = pagedEntities.PageNumber,
            PageSize = pagedEntities.PageSize,
            TotalPages = pagedEntities.TotalPages,
            HasNextPage = pagedEntities.HasNextPage,
            HasPreviousPage = pagedEntities.HasPreviousPage,
            Metadata = new Dictionary<string, object>
            {
                { "availableFilters",    new List<string> { "FullNameAr", "FullNameEn", "LicenseNumber", "Phone", "IdentityNumber" } },
                { "availableSortFields", new List<string> { "FullNameAr", "FullNameEn", "CreatedAt" } }
            }
        };
    }
}
