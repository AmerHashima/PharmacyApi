using Microsoft.Extensions.Options;
using Pharmacy.Application.Interfaces;
using Pharmacy.Application.Options;

namespace Pharmacy.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _uploadRootPath;

    public FileStorageService(IOptions<FileStorageOptions> options)
    {
        _uploadRootPath = options.Value.UploadRootPath;
    }

    public async Task<string> SaveFileAsync(string subFolder, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            var folder = Path.Combine(_uploadRootPath, subFolder);
            Directory.CreateDirectory(folder);

            var fullPath = Path.Combine(folder, fileName);
            await using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await content.CopyToAsync(fs, cancellationToken);

            // Return a URL-friendly relative path starting with /uploads/
            return $"/uploads/{subFolder.Replace('\\', '/')}/{fileName}";
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new InvalidOperationException(
                $"The application does not have write permission to the upload folder '{_uploadRootPath}'. " +
                $"Please ensure the folder exists and the application has write access. Detail: {ex.Message}", ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new InvalidOperationException(
                $"Upload path not found: '{_uploadRootPath}'. Detail: {ex.Message}", ex);
        }
    }

    public Task<bool> DeleteFileAsync(string? relativeUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl))
            return Task.FromResult(false);

        // Convert "/uploads/logos/branches/xxx.jpg" → physical path inside upload root
        // relativeUrl format: /uploads/{subFolder}/{fileName}
        const string prefix = "/uploads/";
        if (!relativeUrl.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(false);

        var relativePart = relativeUrl[prefix.Length..].Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(_uploadRootPath, relativePart);

        if (!File.Exists(fullPath))
            return Task.FromResult(false);

        File.Delete(fullPath);
        return Task.FromResult(true);
    }
}
