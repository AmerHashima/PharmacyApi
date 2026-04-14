using MediatR;
using Pharmacy.Application.DTOs.Branch;

namespace Pharmacy.Application.Commands.Branch;

public record UploadBranchLogoCommand(
    Guid BranchId,
    string FileName,
    Stream FileStream,
    string ContentType) : IRequest<BranchDto>;
