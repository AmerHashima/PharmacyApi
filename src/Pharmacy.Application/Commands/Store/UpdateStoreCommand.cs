using MediatR;
using Pharmacy.Application.DTOs.Store;

namespace Pharmacy.Application.Commands.Store;

public record UpdateStoreCommand(UpdateStoreDto Store) : IRequest<StoreDto>;
