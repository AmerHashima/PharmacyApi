using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Pharmacy.Infrastructure.Services;

/// <summary>
/// Implementation of GS1 barcode parser service
/// Supports various GS1 DataMatrix and QR code formats used in pharmaceutical products
/// </summary>
public class BarcodeParserService : IBarcodeParserService
{
    private readonly Dictionary<string, (Regex Regex, string Substitution)> _patterns;

    public BarcodeParserService()
    {
        // Initialize regex patterns for different GS1 formats
        // AI Codes: 01=GTIN, 21=Serial, 17=Expiry, 10=Batch, 11=Production Date
        _patterns = new Dictionary<string, (Regex, string)>
        {
            ["01_21_17_10_11"] = (
                new Regex(@"(?:01(\d{14})21([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})10([a-zA-Z0-9-]{1,20})(\x1d)?11(\d{6})?)"),
                @"GTIN:$1,SN:$2,EX:$4,BN:$5,PD:$7"
            ),
            ["01_17_10_21"] = (
                new Regex(@"01(\d{14})17(\d{6})10([A-Za-z0-9-]+)21([A-Za-z0-9-]+)"),
                @"GTIN:$1,EX:$2,BN:$3,SN:$4"
            ),
            ["01_21_17_10"] = (
                new Regex(@"(?:01(\d{14})21([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})10([a-zA-Z0-9-]{1,20})(\x1d)?)"),
                @"GTIN:$1,SN:$2,EX:$4,BN:$5"
            ),
            ["01_10_11_17_21"] = (
                new Regex(@"(?:01(\d{14})10([a-zA-Z0-9-]{1,20})(\x1d)?11(\d{6})17(\d{6})21([a-zA-Z0-9-]{1,20})(\x1d)?)"),
                @"GTIN:$1,BN:$2,PD:$4,EX:$5,SN:$6"
            ),
            ["01_10_17_21"] = (
                new Regex(@"(?:01(\d{14})10([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})21([a-zA-Z0-9-]{1,20})(\x1d)?)"),
                @"GTIN:$1,BN:$2,EX:$4,SN:$5"
            ),
            ["01_21_10_17_11"] = (
                new Regex(@"(?:01(\d{14})21([a-zA-Z0-9-]{1,20})(\x1d)?10([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})?11(\d{6}))"),
                @"GTIN:$1,SN:$2,BN:$4,EX:$6,PD:$7"
            ),
            ["01_11_17_10_21"] = (
                new Regex(@"01(\d{14})11(\d{6})17(\d{6})10([a-zA-Z0-9-]{1,20})(\x1d)?21([a-zA-Z0-9-]{1,20})(\x1d)?"),
                @"GTIN:$1,PD:$2,EX:$3,BN:$4,SN:$6"
            ),
            ["01_10_21_17"] = (
                new Regex(@"01(\d{14})10([a-zA-Z0-9-]{1,20})(\x1d)?21([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})"),
                @"GTIN:$1,BN:$2,SN:$4,EX:$6"
            ),
            ["01_17_11_10"] = (
                new Regex(@"01(\d{14})17(\d{6})11(\d{6})10([a-zA-Z0-9-]{1,20})(\x1d)?"),
                @"GTIN:$1,EX:$2,PD:$3,BN:$4"
            ),
            ["01_21_17_11_10"] = (
                new Regex(@"01(\d{14})21([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})11(\d{6})10([a-zA-Z0-9-]{1,20})(\x1d)?"),
                @"GTIN:$1,SN:$2,EX:$4,PD:$5,BN:$6"
            ),
            ["21_01_10_17"] = (
                new Regex(@"21([a-zA-Z0-9-]{1,20})(\x1d)?01(\d{14})10([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})"),
                @"SN:$1,GTIN:$3,BN:$4,EX:$6"
            ),
            ["01_17_11_10_21"] = (
                new Regex(@"01(\d{14})17(\d{6})11(\d{6})10([a-zA-Z0-9-]{1,20})(\x1d)?21([a-zA-Z0-9-]{1,20})(\x1d)?"),
                @"GTIN:$1,EX:$2,PD:$3,BN:$4,SN:$6"
            ),
            ["01_21_10_17"] = (
                new Regex(@"01(\d{14})21([a-zA-Z0-9-]{1,20})(\x1d)?10([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})"),
                @"GTIN:$1,SN:$2,BN:$4,EX:$6"
            ),
            ["01_11_17_21"] = (
                new Regex(@"01(\d{14})11(\d{6})17(\d{6})21([a-zA-Z0-9-]{1,20})(\x1d)?"),
                @"GTIN:$1,PD:$2,EX:$3,SN:$4"
            ),
            ["01_21_11_17_10"] = (
                new Regex(@"01(\d{14})21([a-zA-Z0-9-]{1,20})(\x1d)?11([a-zA-Z0-9-]{1,20})(\x1d)?17(\d{6})10([a-zA-Z0-9-]{1,20})(\x1d)?"),
                @"GTIN:$1,SN:$2,PD:$4,EX:$6,BN:$7"
            )
        };
    }

    public BarcodeParseResponseDto ParseBarcode(string barcodeInput)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(barcodeInput))
            {
                return new BarcodeParseResponseDto
                {
                    Success = false,
                    ErrorMessage = "Barcode input is empty or null"
                };
            }

            // Clean input: remove slashes and spaces
            string cleanInput = barcodeInput.Replace("/", "").Replace(" ", "");

            // Try each pattern until one matches
            string result = null;
            foreach (var pattern in _patterns.Values)
            {
                result = pattern.Regex.Replace(cleanInput, pattern.Substitution);
                if (result.Contains("GTIN"))
                {
                    break;
                }
            }

            if (result == null || !result.Contains("GTIN"))
            {
                return new BarcodeParseResponseDto
                {
                    Success = false,
                    ErrorMessage = "Unable to parse barcode. Format not recognized."
                };
            }

            // Parse the result into a dictionary
            var parsedData = new Dictionary<string, string>();
            foreach (var part in result.Split(','))
            {
                var keyValue = part.Split(':');
                if (keyValue.Length == 2 && !string.IsNullOrEmpty(keyValue[1]))
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    // Format date fields (EX = Expiry, PD = Production Date)
                    if (key == "EX" || key == "PD")
                    {
                        value = FormatDate(value);
                    }

                    parsedData[key] = value;
                }
            }

            // Build response DTO
            return new BarcodeParseResponseDto
            {
                Success = true,
                GTIN = parsedData.GetValueOrDefault("GTIN"),
                SerialNumber = parsedData.GetValueOrDefault("SN"),
                BatchNumber = parsedData.GetValueOrDefault("BN"),
                ExpiryDate = parsedData.GetValueOrDefault("EX"),
                ProductionDate = parsedData.GetValueOrDefault("PD"),
                RawData = parsedData
            };
        }
        catch (Exception ex)
        {
            return new BarcodeParseResponseDto
            {
                Success = false,
                ErrorMessage = $"Error parsing barcode: {ex.Message}"
            };
        }
    }

    private string FormatDate(string dateString)
    {
        try
        {
            // GS1 date format is YYMMDD
            if (string.IsNullOrEmpty(dateString) || dateString.Length != 6)
                return dateString;

            // Handle special case where day is 00 (means end of month)
            if (dateString.EndsWith("00"))
            {
                dateString = dateString.Substring(0, 4) + "01";
            }

            // Parse and format to MM/dd/yyyy
            DateTime parsedDate = DateTime.ParseExact(dateString, "yyMMdd", CultureInfo.InvariantCulture);
            
            // If day was 00, set to last day of month
            if (dateString.EndsWith("01") && dateString.Substring(4, 2) != parsedDate.Day.ToString("00"))
            {
                parsedDate = new DateTime(parsedDate.Year, parsedDate.Month, 
                    DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month));
            }

            return parsedDate.ToString("MM/dd/yyyy");
        }
        catch
        {
            return dateString; // Return original if parsing fails
        }
    }
}
