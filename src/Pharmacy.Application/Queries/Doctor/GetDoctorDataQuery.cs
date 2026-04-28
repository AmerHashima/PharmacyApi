using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Doctor;

namespace Pharmacy.Application.Queries.Doctor;

public record GetDoctorDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<DoctorDto>>;
