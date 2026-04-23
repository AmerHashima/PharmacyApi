using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pharmacy.Application.DTOs.Zatca;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using Zatca.Models;
using Zatca.Services;

namespace Pharmacy.Infrastructure.Integration.Zatca;

public class ZatcaIntegrationService : IZatcaIntegrationService
{
    private const string ZatcaProviderName = "Zatca";
    private const string SettingKeyBaseUrl = "Zatcalink";
    private const string SettingKeyBinarySecurityToken = "InvoiceBinarySecurityToken";
    private const string SettingKeySecret = "InvoiceSecret";
    private const string SettingKeyCertificateContent = "CertificateContent";
    private const string SettingKeyPrivateKeyContent = "PrivateKeyContent";
    private const string SettingKeyEnvironment = "Environment";
    private const string SettingKeyLastInvoicePIH = "LastInvoicePIH";
    private const string SettingKeyIcv = "Icv";

    private const string SimulationBaseUrl = "https://gw-apic-gov.gazt.gov.sa/e-invoicing/simulation/";
    private const string ProductionBaseUrl = "https://gw-apic-gov.gazt.gov.sa/e-invoicing/core/";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IBranchIntegrationSettingRepository _settingRepository;
    private readonly IIntegrationProviderRepository _providerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<ZatcaIntegrationService> _logger;

    public ZatcaIntegrationService(
        IHttpClientFactory httpClientFactory,
        IBranchIntegrationSettingRepository settingRepository,
        IIntegrationProviderRepository providerRepository,
        IBranchRepository branchRepository,
        ILogger<ZatcaIntegrationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settingRepository = settingRepository;
        _providerRepository = providerRepository;
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<ZatcaOnboardResponseDto> OnboardAsync(ZatcaOnboardRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("ZATCA Onboard - Branch: {BranchId}, Portal: {Portal}", request.BranchId, request.InvoicePortalType);

            var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
                ?? throw new InvalidOperationException($"Branch '{request.BranchId}' not found");

            var vatRegNo = branch.VatNumber
                ?? throw new InvalidOperationException($"Branch '{request.BranchId}' has no VAT number configured");

            var csrSetting = new CsrSetting
            {
                NameEn = request.NameEn,
                UnitNameEn = request.UnitNameEn,
                Category = request.Category,
                Address = request.Address,
                InvoicePortalType = request.InvoicePortalType,
                Otp = request.OTP,
                VatRegNo = vatRegNo
            };

            var csrService = new CsrService();
            var certResponse = csrService.CreateCsrAndObtainProductionCsid(csrSetting);

            // Save credentials to BranchIntegrationSettings
            var provider = await GetOrCreateProviderAsync();
            var baseUrl = request.InvoicePortalType == "2" ? SimulationBaseUrl : ProductionBaseUrl;
            var environment = request.InvoicePortalType == "2" ? "Simulation" : "Production";

            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyBaseUrl, baseUrl);
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyBinarySecurityToken, certResponse.BinarySecurityToken);
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeySecret, certResponse.Secret);
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyCertificateContent, certResponse.CertificateContent);
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyPrivateKeyContent, certResponse.PrivateKeyContent);
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyEnvironment, environment);
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyLastInvoicePIH, "0000000000000000000000000000000000000000000000000000000000000000");
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyIcv, "0");

            _logger.LogInformation("ZATCA Onboard success - Branch: {BranchId}, Environment: {Env}", request.BranchId, environment);

            return new ZatcaOnboardResponseDto
            {
                Success = true,
                Message = $"Branch onboarded successfully in {environment} mode",
                BinarySecurityToken = certResponse.BinarySecurityToken,
                Secret = certResponse.Secret,
                CertificateContent = certResponse.CertificateContent,
                PrivateKeyContent = certResponse.PrivateKeyContent
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ZATCA Onboard failed - Branch: {BranchId}", request.BranchId);
            return new ZatcaOnboardResponseDto { Success = false, Message = ex.Message };
        }
    }

    public async Task<ZatcaSubmitInvoiceResponseDto> ReportInvoiceAsync(ZatcaSubmitInvoiceRequestDto request, CancellationToken cancellationToken = default)
    {
        return await SubmitInvoiceAsync(request, "invoices/reporting/single", "Reporting", cancellationToken);
    }

    public async Task<ZatcaSubmitInvoiceResponseDto> ClearInvoiceAsync(ZatcaSubmitInvoiceRequestDto request, CancellationToken cancellationToken = default)
    {
        return await SubmitInvoiceAsync(request, "invoices/clearance/single", "Clearance", cancellationToken);
    }

    private async Task<ZatcaSubmitInvoiceResponseDto> SubmitInvoiceAsync(
        ZatcaSubmitInvoiceRequestDto request,
        string apiPath,
        string operationType,
        CancellationToken cancellationToken)
    {
        try
        {
            var settings = await ResolveSettingsAsync(request.BranchId);
            var provider = await GetOrCreateProviderAsync();

            _logger.LogInformation("ZATCA {Op} - Branch: {BranchId}, InvoiceId: {Id}",
                operationType, request.BranchId, request.InvoiceData.ID);

            // Increment ICV and persist before submission
            var nextIcv = settings.Icv + 1;
            await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyIcv, nextIcv.ToString());

            // Inject credentials and counter from settings
            request.InvoiceData.CertificateContent = settings.CertificateContent;
            request.InvoiceData.PrivateKeyContent = settings.PrivateKeyContent;
            request.InvoiceData.BinarySecurityToken = settings.BinarySecurityToken;
            request.InvoiceData.Secret = settings.Secret;
            request.InvoiceData.ZatcaUrl = new Uri(settings.BaseUrl);
            request.InvoiceData.PIH = settings.LastInvoicePIH;

            // Set invoice type based on operation
            if (operationType == "Reporting")
                request.InvoiceData.InvoiceTypeCodeName = "0200000";

            // Use existing Zatca SDK service to build, sign, and submit
            var submissionService = new InvoiceSubmissionService();
            dynamic result = submissionService.SubmitInvoice(request.InvoiceData);

            // Parse the dynamic result
            var zatcaResponse = result.clearanceResponse as ZatcaValidationResponse;
            string invoiceHash = result.Pih as string ?? string.Empty;
            string errorMsg = result.ErrorMsg as string ?? string.Empty;

            // Persist the new PIH for the next invoice chain
            if (!string.IsNullOrEmpty(invoiceHash))
                await SaveSettingAsync(request.BranchId, provider.Oid, SettingKeyLastInvoicePIH, invoiceHash);

            var warnings = zatcaResponse?.ValidationResults?.WarningMessages?
                .Select(w => new ZatcaValidationMessageDto { Type = w.Type, Code = w.Code, Category = w.Category, Message = w.Message, Status = w.Status })
                .ToList() ?? [];

            var errors = zatcaResponse?.ValidationResults?.ErrorMessages?
                .Select(e => new ZatcaValidationMessageDto { Type = e.Type, Code = e.Code, Category = e.Category, Message = e.Message, Status = e.Status })
                .ToList() ?? [];

            var status = operationType == "Clearance" ? zatcaResponse?.ClearanceStatus : zatcaResponse?.ReportingStatus;

            return new ZatcaSubmitInvoiceResponseDto
            {
                Success = errors.Count == 0,
                Status = status,
                InvoiceHash = invoiceHash,
                ClearedInvoice = zatcaResponse?.ClearedInvoice?.ToString(),
                ErrorMessage = errors.Count > 0 ? string.Join("; ", errors.Select(e => e.Message)) : null,
                Warnings = warnings,
                Errors = errors
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ZATCA {Op} failed - Branch: {BranchId}", operationType, request.BranchId);
            return new ZatcaSubmitInvoiceResponseDto { Success = false, ErrorMessage = ex.Message };
        }
    }

    // ── Settings helpers ──

    private async Task<ZatcaBranchSettings> ResolveSettingsAsync(Guid branchId)
    {
        var provider = await _providerRepository.GetByNameAsync(ZatcaProviderName)
            ?? throw new InvalidOperationException($"Integration provider '{ZatcaProviderName}' not configured. Run onboarding first.");

        var branchSettings = await _settingRepository.GetByBranchIdAsync(branchId);
        var zatcaSettings = branchSettings.Where(s => s.IntegrationProviderId == provider.Oid).ToList();

        if (zatcaSettings.Count == 0)
            throw new InvalidOperationException($"No ZATCA settings for branch '{branchId}'. Run onboarding first.");

        string? Get(string key) => zatcaSettings.FirstOrDefault(s => s.IntegrationKey == key)?.IntegrationValue;

        return new ZatcaBranchSettings(
            Get(SettingKeyBaseUrl) ?? SimulationBaseUrl,
            Get(SettingKeyBinarySecurityToken) ?? throw new InvalidOperationException("ZATCA BinarySecurityToken not configured"),
            Get(SettingKeySecret) ?? throw new InvalidOperationException("ZATCA Secret not configured"),
            Get(SettingKeyCertificateContent) ?? throw new InvalidOperationException("ZATCA CertificateContent not configured"),
            Get(SettingKeyPrivateKeyContent) ?? throw new InvalidOperationException("ZATCA PrivateKeyContent not configured"),
            Get(SettingKeyLastInvoicePIH) ?? "0000000000000000000000000000000000000000000000000000000000000000",
            long.TryParse(Get(SettingKeyIcv), out var icv) ? icv : 0);
    }

    private async Task<Domain.Entities.IntegrationProvider> GetOrCreateProviderAsync()
    {
        var provider = await _providerRepository.GetByNameAsync(ZatcaProviderName);
        if (provider != null) return provider;

        return await _providerRepository.AddAsync(new Domain.Entities.IntegrationProvider
        {
            Name = ZatcaProviderName,
            Description = "ZATCA E-Invoice Integration (Fatoora)",
            Status = 1
        });
    }

    private async Task SaveSettingAsync(Guid branchId, Guid providerId, string key, string? value)
    {
        if (string.IsNullOrEmpty(value)) return;

        var existing = await _settingRepository.GetByBranchIdAsync(branchId);
        var setting = existing.FirstOrDefault(s => s.IntegrationProviderId == providerId && s.IntegrationKey == key);

        if (setting != null)
        {
            setting.IntegrationValue = value;
            await _settingRepository.UpdateAsync(setting);
        }
        else
        {
            await _settingRepository.AddAsync(new Domain.Entities.BranchIntegrationSetting
            {
                BranchId = branchId,
                IntegrationProviderId = providerId,
                IntegrationKey = key,
                IntegrationValue = value,
                Status = 1
            });
        }
    }

    private record ZatcaBranchSettings(string BaseUrl, string BinarySecurityToken, string Secret, string CertificateContent, string PrivateKeyContent, string LastInvoicePIH, long Icv);
}
