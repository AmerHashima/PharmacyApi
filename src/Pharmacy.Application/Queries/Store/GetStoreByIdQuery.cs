using MediatR;
using Pharmacy.Application.DTOs.Store;

namespace Pharmacy.Application.Queries.Store;

public record GetStoreByIdQuery(Guid Id) : IRequest<StoreDto?>;
