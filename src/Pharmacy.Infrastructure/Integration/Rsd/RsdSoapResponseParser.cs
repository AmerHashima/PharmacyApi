using System.Xml.Linq;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Infrastructure.Integration.Rsd;

public static class RsdSoapResponseParser
{
    public static AcceptDispatchResponseDto ParseAcceptDispatchResponse(string rawXml, string dispatchNotificationId)
    {
        try
        {
            var doc = XDocument.Parse(rawXml);
            var body = FindBody(doc);
            if (body == null) return AcceptDispatchError("PARSE_ERROR", "Could not find SOAP Body", rawXml, dispatchNotificationId);
            var fault = ParseFault(body);
            if (fault != null) return AcceptDispatchError("SOAP_FAULT", fault, rawXml, dispatchNotificationId);

            var resp = body.Descendants().FirstOrDefault(e => e.Name.LocalName is "AcceptDispatchServiceResponse" or "AcceptDispatchServiceResult");
            var code = GetValue(resp, "RESPONSECODE", "ResponseCode");
            var msg = GetValue(resp, "RESPONSEMESSAGE", "ResponseMessage");

            return new AcceptDispatchResponseDto
            {
                Success = code is "0" or "00" or null or "",
                ResponseCode = code ?? "OK",
                ResponseMessage = msg ?? "Dispatch accepted successfully",
                RawResponse = rawXml, DispatchNotificationId = dispatchNotificationId
            };
        }
        catch (Exception ex) { return AcceptDispatchError("PARSE_EXCEPTION", $"Failed to parse: {ex.Message}", rawXml, dispatchNotificationId); }
    }

    public static DispatchDetailResponseDto ParseDispatchDetailResponse(string rawXml, string dispatchNotificationId)
    {
        try
        {
            var doc = XDocument.Parse(rawXml);
            var body = FindBody(doc);
            if (body == null) return new DispatchDetailResponseDto { Success = false, ResponseCode = "PARSE_ERROR", ResponseMessage = "Could not find SOAP Body", RawResponse = rawXml, DispatchNotificationId = dispatchNotificationId };
            var fault = ParseFault(body);
            if (fault != null) return new DispatchDetailResponseDto { Success = false, ResponseCode = "SOAP_FAULT", ResponseMessage = fault, RawResponse = rawXml, DispatchNotificationId = dispatchNotificationId };

            var resp = body.Descendants().FirstOrDefault(e => e.Name.LocalName == "DispatchDetailServiceResponse");
            if (resp == null) return new DispatchDetailResponseDto { Success = false, ResponseCode = "NO_RESPONSE", ResponseMessage = "DispatchDetailServiceResponse not found", RawResponse = rawXml, DispatchNotificationId = dispatchNotificationId };

            var products = resp.Descendants().Where(e => e.Name.LocalName == "PRODUCT")
                .Select(p => new DispatchProductDto
                {
                    GTIN = GetValue(p, "GTIN") ?? string.Empty,
                    BatchNumber = GetValue(p, "BN"),
                    ExpiryDate = GetValue(p, "XD"),
                    Quantity = int.TryParse(GetValue(p, "QUANTITY"), out var qty) ? qty : 0,
                    ResponseCode = GetValue(p, "RC")
                }).ToList();

            return new DispatchDetailResponseDto
            {
                Success = true, ResponseCode = "OK",
                ResponseMessage = $"Found {products.Count} product(s)",
                DispatchNotificationId = GetValue(resp, "DISPATCHNOTIFICATIONID") ?? dispatchNotificationId,
                NotificationDate = GetValue(resp, "NOTIFICATIONDATE"),
                FromGLN = GetValue(resp, "FROMGLN"),
                Products = products, RawResponse = rawXml
            };
        }
            catch (Exception ex) { return new DispatchDetailResponseDto { Success = false, ResponseCode = "PARSE_EXCEPTION", ResponseMessage = $"Failed to parse: {ex.Message}", RawResponse = rawXml, DispatchNotificationId = dispatchNotificationId }; }
    }

    public static AcceptBatchResponseDto ParseAcceptBatchResponse(string rawXml)
    {
        try
        {
            var doc = XDocument.Parse(rawXml);
            var body = FindBody(doc);
            if (body == null) return new AcceptBatchResponseDto { Success = false, ResponseCode = "PARSE_ERROR", ResponseMessage = "Could not find SOAP Body", RawResponse = rawXml };
            var fault = ParseFault(body);
            if (fault != null) return new AcceptBatchResponseDto { Success = false, ResponseCode = "SOAP_FAULT", ResponseMessage = fault, RawResponse = rawXml };

            var resp = body.Descendants().FirstOrDefault(e => e.Name.LocalName == "AcceptBatchServiceResponse");
            if (resp == null) return new AcceptBatchResponseDto { Success = false, ResponseCode = "NO_RESPONSE", ResponseMessage = "AcceptBatchServiceResponse not found", RawResponse = rawXml };

            var products = ParseBatchProducts(resp);
            var notifId = GetValue(resp, "NOTIFICATIONID");

            return new AcceptBatchResponseDto
            {
                Success = true, ResponseCode = "OK",
                ResponseMessage = $"Batch accepted with {products.Count} product(s), NotificationId: {notifId}",
                NotificationId = notifId, Products = products, RawResponse = rawXml
            };
        }
        catch (Exception ex) { return new AcceptBatchResponseDto { Success = false, ResponseCode = "PARSE_EXCEPTION", ResponseMessage = $"Failed to parse: {ex.Message}", RawResponse = rawXml }; }
    }

    public static PharmacySaleResponseDto ParsePharmacySaleResponse(string rawXml)
    {
        try
        {
            var doc = XDocument.Parse(rawXml);
            var body = FindBody(doc);
            if (body == null) return new PharmacySaleResponseDto { Success = false, ResponseCode = "PARSE_ERROR", ResponseMessage = "Could not find SOAP Body", RawResponse = rawXml };
            var fault = ParseFault(body);
            if (fault != null) return new PharmacySaleResponseDto { Success = false, ResponseCode = "SOAP_FAULT", ResponseMessage = fault, RawResponse = rawXml };

            var resp = body.Descendants().FirstOrDefault(e => e.Name.LocalName == "AcceptServiceResponse");
            if (resp == null) return new PharmacySaleResponseDto { Success = false, ResponseCode = "NO_RESPONSE", ResponseMessage = "AcceptServiceResponse not found", RawResponse = rawXml };

            var notifId = GetValue(resp, "NOTIFICATIONID");

            var products = resp.Descendants().Where(e => e.Name.LocalName == "PRODUCT")
                .Select(p => new PharmacySaleProductResultDto
                {
                    GTIN = GetValue(p, "GTIN") ?? string.Empty,
                    BatchNumber = GetValue(p, "BN"),
                    ExpiryDate = GetValue(p, "XD"),
                    SerialNumber = GetValue(p, "SN"),
                    ResponseCode = GetValue(p, "RC")
                }).ToList();

            return new PharmacySaleResponseDto
            {
                Success = true, ResponseCode = "OK",
                ResponseMessage = $"Sale reported with {products.Count} product(s), NotificationId: {notifId}",
                NotificationId = notifId, Products = products, RawResponse = rawXml
            };
        }
        catch (Exception ex) { return new PharmacySaleResponseDto { Success = false, ResponseCode = "PARSE_EXCEPTION", ResponseMessage = $"Failed to parse: {ex.Message}", RawResponse = rawXml }; }
    }

    public static PharmacySaleCancelResponseDto ParsePharmacySaleCancelResponse(string rawXml)
    {
        try
        {
            var doc = XDocument.Parse(rawXml);
            var body = FindBody(doc);
            if (body == null) return new PharmacySaleCancelResponseDto { Success = false, ResponseCode = "PARSE_ERROR", ResponseMessage = "Could not find SOAP Body", RawResponse = rawXml };
            var fault = ParseFault(body);
            if (fault != null) return new PharmacySaleCancelResponseDto { Success = false, ResponseCode = "SOAP_FAULT", ResponseMessage = fault, RawResponse = rawXml };

            var resp = body.Descendants().FirstOrDefault(e => e.Name.LocalName == "AcceptBatchServiceResponse");
            if (resp == null) return new PharmacySaleCancelResponseDto { Success = false, ResponseCode = "NO_RESPONSE", ResponseMessage = "AcceptBatchServiceResponse not found", RawResponse = rawXml };

            var notifId = GetValue(resp, "NOTIFICATIONID");

            var products = resp.Descendants().Where(e => e.Name.LocalName == "PRODUCT")
                .Select(p => new PharmacySaleCancelProductResultDto
                {
                    GTIN = GetValue(p, "GTIN") ?? string.Empty,
                    BatchNumber = GetValue(p, "BN"),
                    ExpiryDate = GetValue(p, "XD"),
                    Quantity = int.TryParse(GetValue(p, "QUANTITY"), out var qty) ? qty : 0,
                    ResponseCode = GetValue(p, "RC")
                }).ToList();

            return new PharmacySaleCancelResponseDto
            {
                Success = true, ResponseCode = "OK",
                ResponseMessage = $"Sale cancel reported with {products.Count} product(s), NotificationId: {notifId}",
                NotificationId = notifId, Products = products, RawResponse = rawXml
            };
        }
        catch (Exception ex) { return new PharmacySaleCancelResponseDto { Success = false, ResponseCode = "PARSE_EXCEPTION", ResponseMessage = $"Failed to parse: {ex.Message}", RawResponse = rawXml }; }
    }

    public static StakeholderListResponseDto ParseStakeholderListResponse(string rawXml)
    {
        try
        {
            var doc = XDocument.Parse(rawXml);
            var body = FindBody(doc);
            if (body == null) return new StakeholderListResponseDto { Success = false, ResponseCode = "PARSE_ERROR", ResponseMessage = "Could not find SOAP Body", RawResponse = rawXml };
            var fault = ParseFault(body);
            if (fault != null) return new StakeholderListResponseDto { Success = false, ResponseCode = "SOAP_FAULT", ResponseMessage = fault, RawResponse = rawXml };

            var resp = body.Descendants().FirstOrDefault(e => e.Name.LocalName == "StakeholderListServiceResponse");
            if (resp == null) return new StakeholderListResponseDto { Success = false, ResponseCode = "NO_RESPONSE", ResponseMessage = "StakeholderListServiceResponse not found", RawResponse = rawXml };

            var stakeholders = resp.Descendants()
                .Where(e => e.Name.LocalName == "STAKEHOLDER")
                .Select(s => new RsdStakeholderDto
                {
                    GLN = GetValue(s, "GLN") ?? string.Empty,
                    StakeholderName = GetValue(s, "STAKEHOLDERNAME"),
                    CityName = GetValue(s, "CITYNAME"),
                    Address = GetValue(s, "ADDRESS"),
                    IsActive = GetValue(s, "ISACTIVE")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false
                })
                .Where(s => !string.IsNullOrEmpty(s.GLN))
                .ToList();

            return new StakeholderListResponseDto
            {
                Success = true,
                ResponseCode = "OK",
                ResponseMessage = $"Found {stakeholders.Count} stakeholder(s)",
                TotalCount = stakeholders.Count,
                Stakeholders = stakeholders,
                RawResponse = rawXml
            };
        }
        catch (Exception ex) { return new StakeholderListResponseDto { Success = false, ResponseCode = "PARSE_EXCEPTION", ResponseMessage = $"Failed to parse: {ex.Message}", RawResponse = rawXml }; }
    }

    public static ReturnBatchResponseDto ParseReturnBatchResponse(string rawXml)
    {
        try
        {
            var doc = XDocument.Parse(rawXml);
            var body = FindBody(doc);
            if (body == null) return new ReturnBatchResponseDto { Success = false, ResponseCode = "PARSE_ERROR", ResponseMessage = "Could not find SOAP Body", RawResponse = rawXml };
            var fault = ParseFault(body);
            if (fault != null) return new ReturnBatchResponseDto { Success = false, ResponseCode = "SOAP_FAULT", ResponseMessage = fault, RawResponse = rawXml };

            var resp = body.Descendants().FirstOrDefault(e => e.Name.LocalName == "ReturnBatchServiceResponse");
            if (resp == null) return new ReturnBatchResponseDto { Success = false, ResponseCode = "NO_RESPONSE", ResponseMessage = "ReturnBatchServiceResponse not found", RawResponse = rawXml };

            var notifId = GetValue(resp, "NOTIFICATIONID");

            var products = resp.Descendants().Where(e => e.Name.LocalName == "PRODUCT")
                .Select(p => new ReturnBatchProductResultDto
                {
                    GTIN = GetValue(p, "GTIN") ?? string.Empty,
                    BatchNumber = GetValue(p, "BN"),
                    ExpiryDate = GetValue(p, "XD"),
                    Quantity = int.TryParse(GetValue(p, "QUANTITY"), out var qty) ? qty : 0,
                    ResponseCode = GetValue(p, "RC")
                }).ToList();

            return new ReturnBatchResponseDto
            {
                Success = true, ResponseCode = "OK",
                ResponseMessage = $"Return batch submitted with {products.Count} product(s), NotificationId: {notifId}",
                NotificationId = notifId, Products = products, RawResponse = rawXml
            };
        }
        catch (Exception ex) { return new ReturnBatchResponseDto { Success = false, ResponseCode = "PARSE_EXCEPTION", ResponseMessage = $"Failed to parse: {ex.Message}", RawResponse = rawXml }; }
    }

    // ── Shared helpers ──

    private static XElement? FindBody(XDocument doc) =>
        doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "Body");

    private static string? ParseFault(XElement body) =>
        body.Descendants().FirstOrDefault(e => e.Name.LocalName == "Fault")
            ?.Descendants().FirstOrDefault(e => e.Name.LocalName == "faultstring")?.Value;

    private static string? GetValue(XElement? parent, params string[] localNames)
    {
        if (parent == null) return null;
        foreach (var name in localNames)
        {
            var el = parent.Descendants().FirstOrDefault(e => e.Name.LocalName == name);
            if (el != null) return el.Value;
        }
        return null;
    }

    private static List<AcceptBatchProductResultDto> ParseBatchProducts(XElement resp) =>
        resp.Descendants().Where(e => e.Name.LocalName == "PRODUCT")
            .Select(p => new AcceptBatchProductResultDto
            {
                GTIN = GetValue(p, "GTIN") ?? string.Empty,
                BatchNumber = GetValue(p, "BN"),
                ExpiryDate = GetValue(p, "XD"),
                Quantity = int.TryParse(GetValue(p, "QUANTITY"), out var qty) ? qty : 0,
                ResponseCode = GetValue(p, "RC")
            }).ToList();

    private static AcceptDispatchResponseDto AcceptDispatchError(string code, string msg, string rawXml, string dispatchId) =>
        new() { Success = false, ResponseCode = code, ResponseMessage = msg, RawResponse = rawXml, DispatchNotificationId = dispatchId };
}