namespace Pharmacy.Application.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// Saves a file under a sub-folder and returns the relative URL path.
    /// </summary>
    Task<string> SaveFileAsync(string subFolder, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file by its relative URL path. Returns false if the file did not exist.
    /// </summary>
    Task<bool> DeleteFileAsync(string? relativeUrl, CancellationToken cancellationToken = default);
}
