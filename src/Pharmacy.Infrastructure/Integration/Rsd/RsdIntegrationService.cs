using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Infrastructure.Integration.Rsd;

public class RsdIntegrationService : IRsdIntegrationService
{
    private const string RsdProviderName = "RSD";
    private const string SettingKeyBaseUrl = "BaseUrl";
    private const string SettingKeyUsername = "Username";
    private const string SettingKeyPassword = "Password";

    private const string DefaultBaseUrl = "https://rsd.sfda.gov.sa/ws";

    // Service paths
    private const string AcceptDispatchPath = "/AcceptDispatchService/AcceptDispatchService";
    private const string DispatchDetailPath = "/DispatchDetailService/DispatchDetailService";
    private const string AcceptBatchPath = "/AcceptBatchService/AcceptBatchService";
    private const string PharmacySalePath = "/PharmacySaleService/PharmacySaleService";
    private const string PharmacySaleCancelPath = "/PharmacySaleCancelService/PharmacySaleCancelService";
    private const string StakeholderListPath = "/StakeholderListService/StakeholderListService";
    private const string ReturnBatchPath = "/ReturnBatchService/ReturnBatchService";
    private const string DrugListPath = "/DrugListService/DrugListService";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IBranchIntegrationSettingRepository _settingRepository;
    private readonly IIntegrationProviderRepository _providerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<RsdIntegrationService> _logger;

    public RsdIntegrationService(
        IHttpClientFactory httpClientFactory,
        IBranchIntegrationSettingRepository settingRepository,
        IIntegrationProviderRepository providerRepository,
        IBranchRepository branchRepository,
        ILogger<RsdIntegrationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settingRepository = settingRepository;
        _providerRepository = providerRepository;
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<AcceptDispatchResponseDto> AcceptDispatchAsync(AcceptDispatchRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(AcceptDispatchPath);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildAcceptDispatchEnvelope(request.DispatchNotificationId);

        _logger.LogInformation("RSD AcceptDispatch - Branch: {BranchId}, DispatchId: {DispatchId}", request.BranchId, request.DispatchNotificationId);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "AcceptDispatchService", cancellationToken);
        var result = RsdSoapResponseParser.ParseAcceptDispatchResponse(rawResponse, request.DispatchNotificationId);

        _logger.LogInformation("RSD AcceptDispatch - Success: {Success}, Code: {Code}", result.Success, result.ResponseCode);
        return result;
    }

    public async Task<DispatchDetailResponseDto> GetDispatchDetailAsync(DispatchDetailRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(DispatchDetailPath);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildDispatchDetailEnvelope(request.DispatchNotificationId);

        _logger.LogInformation("RSD DispatchDetail - Branch: {BranchId}, DispatchId: {DispatchId}", request.BranchId, request.DispatchNotificationId);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "DispatchDetailService", cancellationToken);
        var result = RsdSoapResponseParser.ParseDispatchDetailResponse(rawResponse, request.DispatchNotificationId);

        _logger.LogInformation("RSD DispatchDetail - Success: {Success}, Products: {Count}", result.Success, result.Products.Count);
        return result;
    }

    public async Task<AcceptBatchResponseDto> AcceptBatchAsync(AcceptBatchRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(AcceptBatchPath);
        var fromGln = await ResolveGlnAsync(request.BranchId, request.FromGLN, cancellationToken);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildAcceptBatchEnvelope(fromGln, request.Products);

        _logger.LogInformation("RSD AcceptBatch - Branch: {BranchId}, GLN: {GLN}, Products: {Count}", request.BranchId, fromGln, request.Products.Count);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "AcceptBatchService", cancellationToken);
        var result = RsdSoapResponseParser.ParseAcceptBatchResponse(rawResponse);

        _logger.LogInformation("RSD AcceptBatch - NotificationId: {Id}, Success: {Success}", result.NotificationId, result.Success);
        return result;
    }

    public async Task<PharmacySaleResponseDto> PharmacySaleAsync(PharmacySaleRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(PharmacySalePath);
        var fromGln = await ResolveGlnAsync(request.BranchId, request.FromGLN, cancellationToken);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildPharmacySaleEnvelope(fromGln, request.Products);

        _logger.LogInformation("RSD PharmacySale - Branch: {BranchId}, GLN: {GLN}, Products: {Count}", request.BranchId, fromGln, request.Products.Count);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "PharmacySaleService", cancellationToken);
        var result = RsdSoapResponseParser.ParsePharmacySaleResponse(rawResponse);

        _logger.LogInformation("RSD PharmacySale - NotificationId: {Id}, Success: {Success}, Products: {Count}",
            result.NotificationId, result.Success, result.Products.Count);
        return result;
    }

    public async Task<PharmacySaleCancelResponseDto> PharmacySaleCancelAsync(PharmacySaleCancelRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(PharmacySaleCancelPath);
        var fromGln = await ResolveGlnAsync(request.BranchId, request.FromGLN, cancellationToken);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildPharmacySaleCancelEnvelope(fromGln, request.Products);

        _logger.LogInformation("RSD PharmacySaleCancel - Branch: {BranchId}, GLN: {GLN}, Products: {Count}", request.BranchId, fromGln, request.Products.Count);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "PharmacySaleCancelService", cancellationToken);
        var result = RsdSoapResponseParser.ParsePharmacySaleCancelResponse(rawResponse);

        _logger.LogInformation("RSD PharmacySaleCancel - NotificationId: {Id}, Success: {Success}, Products: {Count}",
            result.NotificationId, result.Success, result.Products.Count);
        return result;
    }

    public async Task<StakeholderListResponseDto> GetStakeholderListAsync(StakeholderListRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(StakeholderListPath);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildStakeholderListEnvelope(request.StakeholderType, request.GetAll, request.CityId);

            //_logger.LogInformation("RSD StakeholderList - Branch: {BranchId}, Type: {Type}, GetAll: {GetAll}",
            //    request.BranchId, request.StakeholderType, request.GetAll);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "StakeholderListService", cancellationToken);
        var result = RsdSoapResponseParser.ParseStakeholderListResponse(rawResponse);

        //_logger.LogInformation("RSD StakeholderList - Success: {Success}, Count: {Count}",
        //    result.Success, result.TotalCount);
        return result;
    }

    public async Task<ReturnBatchResponseDto> ReturnBatchAsync(ReturnBatchRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(ReturnBatchPath);
        var toGln = await ResolveGlnAsync(request.BranchId, request.ToGLN, cancellationToken);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildReturnBatchEnvelope(toGln, request.Products);

        _logger.LogInformation("RSD ReturnBatch - Branch: {BranchId}, ToGLN: {GLN}, Products: {Count}",
            request.BranchId, toGln, request.Products.Count);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "ReturnBatchService", cancellationToken);
        var result = RsdSoapResponseParser.ParseReturnBatchResponse(rawResponse);

        _logger.LogInformation("RSD ReturnBatch - NotificationId: {Id}, Success: {Success}, Products: {Count}",
            result.NotificationId, result.Success, result.Products.Count);
        return result;
    }

    // ── Private helpers ──

    public async Task<List<DrugListItemDto>> GetDrugListAsync(DrugListRequestDto request, CancellationToken cancellationToken = default)
    {
        var settings = await ResolveSettingsAsync(request.BranchId);
        var endpoint = settings.BuildUrl(DrugListPath);
        var soapEnvelope = RsdSoapEnvelopeBuilder.BuildDrugListEnvelope(request.DrugStatus);

        _logger.LogInformation("RSD DrugList - Branch: {BranchId}, DrugStatus: {Status}", request.BranchId, request.DrugStatus);

        var rawResponse = await SendSoapRequestAsync(settings, endpoint, soapEnvelope, "DrugListService", cancellationToken);
        var drugs = RsdSoapResponseParser.ParseDrugListResponse(rawResponse);

        _logger.LogInformation("RSD DrugList - Returned {Count} drug(s)", drugs.Count);
        return drugs;
    }

    private async Task<string> ResolveGlnAsync(Guid branchId, string? providedGln, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(providedGln))
            return providedGln;

        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        return branch?.GLN
            ?? throw new InvalidOperationException($"Branch '{branchId}' has no GLN configured. Provide FromGLN or set the branch GLN.");
    }

    private async Task<RsdBranchSettings> ResolveSettingsAsync(Guid branchId)
    {
        var provider = await _providerRepository.GetByNameAsync(RsdProviderName)
            ?? throw new InvalidOperationException($"Integration provider '{RsdProviderName}' is not configured.");

        var branchSettings = await _settingRepository.GetByBranchIdAsync(branchId);
        var rsdSettings = branchSettings.Where(s => s.IntegrationProviderId == provider.Oid).ToList();

        if (rsdSettings.Count == 0)
            throw new InvalidOperationException($"No RSD settings found for branch '{branchId}'.");

        var baseUrl = rsdSettings.FirstOrDefault(s => s.IntegrationKey == SettingKeyBaseUrl)?.IntegrationValue ?? DefaultBaseUrl;
        var username = rsdSettings.FirstOrDefault(s => s.IntegrationKey == SettingKeyUsername)?.IntegrationValue;
        var password = rsdSettings.FirstOrDefault(s => s.IntegrationKey == SettingKeyPassword)?.IntegrationValue;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new InvalidOperationException($"RSD credentials not configured for branch '{branchId}'.");

        return new RsdBranchSettings(baseUrl.TrimEnd('/'), username, password);
    }

    private async Task<string> SendSoapRequestAsync(RsdBranchSettings settings, string endpoint, string soapEnvelope, string soapAction, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("RsdClient");
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);

        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.Username}:{settings.Password}"));
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        httpRequest.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml") { CharSet = "utf-8" };
        httpRequest.Headers.Add("SOAPAction", $"\"{soapAction}\"");

        _logger.LogDebug("RSD Request to {Endpoint}:\n{Body}", endpoint, soapEnvelope);

        var response = await client.SendAsync(httpRequest, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogDebug("RSD Response ({StatusCode}):\n{Body}", response.StatusCode, responseBody);

        if (!response.IsSuccessStatusCode)
            _logger.LogWarning("RSD returned HTTP {StatusCode}: {Body}", response.StatusCode, responseBody);

        return responseBody;
    }

    private record RsdBranchSettings(string BaseUrl, string Username, string Password)
    {
        public string BuildUrl(string servicePath) => $"{BaseUrl}{servicePath}";
    }
}