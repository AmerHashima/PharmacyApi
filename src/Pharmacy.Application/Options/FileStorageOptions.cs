namespace Pharmacy.Application.Options;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";

    /// <summary>Absolute physical root path where uploads are stored.</summary>
    public string UploadRootPath { get; set; } = string.Empty;

    /// <summary>Allowed image extensions (e.g. ".jpg", ".png").</summary>
    public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".webp"];

    /// <summary>Maximum allowed file size in bytes (default 2 MB).</summary>
    public long MaxFileSizeBytes { get; set; } = 2 * 1024 * 1024;
}
