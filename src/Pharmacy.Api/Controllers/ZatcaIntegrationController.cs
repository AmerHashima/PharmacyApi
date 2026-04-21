using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Zatca;
using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for ZATCA E-Invoice (Fatoora) integration operations
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class ZatcaIntegrationController : BaseApiController
{
    private readonly IMediator _mediator;

    public ZatcaIntegrationController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Onboard a branch with ZATCA — generates CSR, obtains compliance &amp; production CSID,
    /// and saves credentials to BranchIntegrationSettings
    /// </summary>
    [HttpPost("onboard")]
    public async Task<ActionResult<ApiResponse<ZatcaOnboardResponseDto>>> Onboard([FromBody] ZatcaOnboardRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new ZatcaOnboardCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Branch onboarded with ZATCA successfully")
                : ErrorResponse<ZatcaOnboardResponseDto>(result.Message ?? "Onboarding failed", 400);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ZatcaOnboardResponseDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Report a simplified invoice to ZATCA (Phase 2 — B2C).
    /// Credentials are loaded from BranchIntegrationSettings automatically.
    /// </summary>
    [HttpPost("report-invoice")]
    public async Task<ActionResult<ApiResponse<ZatcaSubmitInvoiceResponseDto>>> ReportInvoice([FromBody] ZatcaSubmitInvoiceRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new ZatcaReportInvoiceCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Invoice reported to ZATCA successfully")
                : ErrorResponse<ZatcaSubmitInvoiceResponseDto>(result.ErrorMessage ?? "Reporting failed", 400);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ZatcaSubmitInvoiceResponseDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Report a sales invoice to ZATCA by its ID.
    /// All invoice data (branch, customer, items) is loaded automatically from the database.
    /// </summary>
    [HttpPost("report-sales-invoice/{invoiceId:guid}")]
    public async Task<ActionResult<ApiResponse<ZatcaSubmitInvoiceResponseDto>>> ReportSalesInvoice(Guid invoiceId)
    {
        try
        {
            var result = await _mediator.Send(new ZatcaReportSalesInvoiceCommand(invoiceId));
            return result.Success
                ? SuccessResponse(result, "Invoice reported to ZATCA successfully")
                : ErrorResponse<ZatcaSubmitInvoiceResponseDto>(result.ErrorMessage ?? "Reporting failed", 400);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ZatcaSubmitInvoiceResponseDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Clear a standard invoice with ZATCA (Phase 2 — B2B).
    /// Credentials are loaded from BranchIntegrationSettings automatically.
    /// </summary>
    [HttpPost("clear-invoice")]
    public async Task<ActionResult<ApiResponse<ZatcaSubmitInvoiceResponseDto>>> ClearInvoice([FromBody] ZatcaSubmitInvoiceRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new ZatcaClearInvoiceCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Invoice cleared with ZATCA successfully")
                : ErrorResponse<ZatcaSubmitInvoiceResponseDto>(result.ErrorMessage ?? "Clearance failed", 400);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ZatcaSubmitInvoiceResponseDto>(ex.Message, 400);
        }
    }
}
