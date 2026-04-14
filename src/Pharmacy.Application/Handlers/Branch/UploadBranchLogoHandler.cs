using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Pharmacy.Application.Commands.Branch;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Application.Interfaces;
using Pharmacy.Application.Options;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Branch;

public class UploadBranchLogoHandler : IRequestHandler<UploadBranchLogoCommand, BranchDto>
{
    private readonly IBranchRepository _repository;
    private readonly IFileStorageService _fileStorage;
    private readonly IMapper _mapper;
    private readonly FileStorageOptions _options;

    public UploadBranchLogoHandler(
        IBranchRepository repository,
        IFileStorageService fileStorage,
        IMapper mapper,
        IOptions<FileStorageOptions> options)
    {
        _repository = repository;
        _fileStorage = fileStorage;
        _mapper = mapper;
        _options = options.Value;
    }

    public async Task<BranchDto> Handle(UploadBranchLogoCommand request, CancellationToken cancellationToken)
    {
        var branch = await _repository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new KeyNotFoundException($"Branch with ID '{request.BranchId}' not found");

        var ext = Path.GetExtension(request.FileName).ToLowerInvariant();
        if (!_options.AllowedExtensions.Contains(ext))
            throw new InvalidOperationException($"File extension '{ext}' is not allowed. Allowed: {string.Join(", ", _options.AllowedExtensions)}");

        // Delete old logo if one exists
        if (!string.IsNullOrWhiteSpace(branch.LogoImage))
            await _fileStorage.DeleteFileAsync(branch.LogoImage, cancellationToken);

        // Save new logo — file named by branchId to avoid conflicts
        var fileName = $"{request.BranchId}{ext}";
        var relativeUrl = await _fileStorage.SaveFileAsync("logos/branches", fileName, request.FileStream, request.ContentType, cancellationToken);

        branch.LogoImage = relativeUrl;
        branch.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(branch, cancellationToken);

        return _mapper.Map<BranchDto>(branch);
    }
}
