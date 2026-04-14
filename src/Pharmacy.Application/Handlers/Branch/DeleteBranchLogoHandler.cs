using MediatR;
using Pharmacy.Application.Commands.Branch;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Branch;

public class DeleteBranchLogoHandler : IRequestHandler<DeleteBranchLogoCommand, bool>
{
    private readonly IBranchRepository _repository;
    private readonly IFileStorageService _fileStorage;

    public DeleteBranchLogoHandler(IBranchRepository repository, IFileStorageService fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(DeleteBranchLogoCommand request, CancellationToken cancellationToken)
    {
        var branch = await _repository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new KeyNotFoundException($"Branch with ID '{request.BranchId}' not found");

        if (string.IsNullOrWhiteSpace(branch.LogoImage))
            return false;

        await _fileStorage.DeleteFileAsync(branch.LogoImage, cancellationToken);

        branch.LogoImage = null;
        branch.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(branch, cancellationToken);

        return true;
    }
}
