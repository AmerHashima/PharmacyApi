using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Store;

namespace Pharmacy.Application.Queries.Store;

public record GetStoreDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<StoreDto>>;
