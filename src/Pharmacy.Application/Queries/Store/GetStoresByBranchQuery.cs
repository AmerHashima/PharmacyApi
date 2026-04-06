using MediatR;
using Pharmacy.Application.DTOs.Store;

namespace Pharmacy.Application.Queries.Store;

public record GetStoresByBranchQuery(Guid BranchId) : IRequest<IEnumerable<StoreDto>>;
