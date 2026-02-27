using Pharmacy.Application.DTOs.Product;

namespace Pharmacy.Application.Services;

/// <summary>
/// Service for parsing GS1 barcodes (DataMatrix/QR codes) on pharmaceutical products
/// </summary>
public interface IBarcodeParserService
{
    /// <summary>
    /// Parse GS1 barcode string and extract product information
    /// </summary>
    /// <param name="barcodeInput">Raw barcode string</param>
    /// <returns>Parsed barcode data</returns>
    BarcodeParseResponseDto ParseBarcode(string barcodeInput);
}
