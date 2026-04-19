namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// Response DTO for the DrugList sync operation.
/// Summarises what was fetched from RSD and what was updated in the local product catalogue.
/// </summary>
public class DrugListSyncResponseDto
{
    public bool Success { get; set; }
    public string ResponseCode { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;

    /// <summary>Total drug records returned by RSD</summary>
    public int TotalFromRsd { get; set; }

    /// <summary>Products matched and updated in the local database</summary>
    public int UpdatedCount { get; set; }

    /// <summary>Drug records from RSD that had no matching product (by GTIN or RegistrationNumber)</summary>
    public int NotFoundCount { get; set; }

    /// <summary>GTINs / registration numbers that were skipped (no identifier to match on)</summary>
    public int SkippedCount { get; set; }

    /// <summary>Products that were updated</summary>
    public List<DrugListSyncResultItemDto> UpdatedProducts { get; set; } = [];

    /// <summary>Drug entries from RSD with no matching local product</summary>
    public List<DrugListItemDto> NotFoundDrugs { get; set; } = [];

    public string? RawResponse { get; set; }
}
