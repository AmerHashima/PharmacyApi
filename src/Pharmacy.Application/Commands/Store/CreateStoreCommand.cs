using MediatR;
using Pharmacy.Application.DTOs.Store;

namespace Pharmacy.Application.Commands.Store;

public record CreateStoreCommand(CreateStoreDto Store) : IRequest<StoreDto>;
