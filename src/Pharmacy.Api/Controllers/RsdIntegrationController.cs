using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Queries.Rsd;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for SFDA RSD (Drug Track &amp; Trace) integration operations
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class RsdIntegrationController : BaseApiController
{
    private readonly IMediator _mediator;

    public RsdIntegrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Accept a dispatch notification from SFDA RSD
    /// </summary>
    [HttpPost("accept-dispatch")]
    public async Task<ActionResult<ApiResponse<AcceptDispatchResponseDto>>> AcceptDispatch([FromBody] AcceptDispatchRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new AcceptDispatchCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Dispatch accepted successfully")
                : ErrorResponse<AcceptDispatchResponseDto>(result.ResponseMessage ?? "RSD request failed", 400);
        }
        catch (InvalidOperationException ex) { return ErrorResponse<AcceptDispatchResponseDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Get dispatch detail (product list) from SFDA RSD
    /// </summary>
    [HttpPost("dispatch-detail")]
    public async Task<ActionResult<ApiResponse<DispatchDetailResponseDto>>> GetDispatchDetail([FromBody] DispatchDetailRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new GetDispatchDetailCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Dispatch detail retrieved successfully")
                : ErrorResponse<DispatchDetailResponseDto>(result.ResponseMessage ?? "RSD request failed", 400);
        }
        catch (InvalidOperationException ex) { return ErrorResponse<DispatchDetailResponseDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Accept a batch of products into SFDA RSD
    /// </summary>
    [HttpPost("accept-batch")]
    public async Task<ActionResult<ApiResponse<AcceptBatchResponseDto>>> AcceptBatch([FromBody] AcceptBatchRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new AcceptBatchCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Batch accepted successfully")
                : ErrorResponse<AcceptBatchResponseDto>(result.ResponseMessage ?? "RSD request failed", 400);
        }
        catch (InvalidOperationException ex) { return ErrorResponse<AcceptBatchResponseDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Report a pharmacy sale (with serial numbers) to SFDA RSD
    /// </summary>
    [HttpPost("pharmacy-sale")]
    public async Task<ActionResult<ApiResponse<PharmacySaleResponseDto>>> PharmacySale([FromBody] PharmacySaleRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new PharmacySaleCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Pharmacy sale reported successfully")
                : ErrorResponse<PharmacySaleResponseDto>(result.ResponseMessage ?? "RSD request failed", 400);
        }
        catch (InvalidOperationException ex) { return ErrorResponse<PharmacySaleResponseDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Cancel a pharmacy sale in SFDA RSD (by batch quantity)
    /// </summary>
    [HttpPost("pharmacy-sale-cancel")]
    public async Task<ActionResult<ApiResponse<PharmacySaleCancelResponseDto>>> PharmacySaleCancel([FromBody] PharmacySaleCancelRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new PharmacySaleCancelCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Pharmacy sale cancelled successfully")
                : ErrorResponse<PharmacySaleCancelResponseDto>(result.ResponseMessage ?? "RSD request failed", 400);
        }
        catch (InvalidOperationException ex) { return ErrorResponse<PharmacySaleCancelResponseDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Get stakeholder list from SFDA RSD and save new stakeholders to database.
    /// StakeholderType: 1=Pharmacy, 2=Supplier, 3=Distributor, 4=Manufacturer, 5=Wholesaler
    /// </summary>
    [HttpPost("stakeholder-list")]
    public async Task<ActionResult<ApiResponse<StakeholderListResponseDto>>> GetStakeholderList([FromBody] StakeholderListRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new GetStakeholderListCommand(dto));
            return result.Success
                ? SuccessResponse(result, result.ResponseMessage ?? "Stakeholder list retrieved successfully")
                : ErrorResponse<StakeholderListResponseDto>(result.ResponseMessage ?? "RSD request failed", 400);
        }
        catch (InvalidOperationException ex) { return ErrorResponse<StakeholderListResponseDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Return products by batch number to a stakeholder via SFDA RSD.
    /// If ToGLN is not provided, it will be resolved from the branch GLN.
    /// </summary>
    [HttpPost("return-batch")]
    public async Task<ActionResult<ApiResponse<ReturnBatchResponseDto>>> ReturnBatch([FromBody] ReturnBatchRequestDto dto)
    {
        try
        {
            var result = await _mediator.Send(new ReturnBatchCommand(dto));
            return result.Success
                ? SuccessResponse(result, "Return batch submitted successfully")
                : ErrorResponse<ReturnBatchResponseDto>(result.ResponseMessage ?? "RSD request failed", 400);
        }
        catch (InvalidOperationException ex) { return ErrorResponse<ReturnBatchResponseDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Query RSD operation logs with advanced filtering, sorting, and pagination.
    /// Supports filtering by OperationTypeId, BranchId, Success, GLN, NotificationId, RequestedAt, etc.
    /// </summary>
    [HttpPost("operation-logs/query")]
    public async Task<ActionResult<ApiResponse<PagedResult<RsdOperationLogDto>>>> GetOperationLogs([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetRsdOperationLogDataQuery(request));
            return SuccessResponse(result, "RSD operation logs retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<RsdOperationLogDto>>($"Error retrieving RSD operation logs: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get a single RSD operation log by ID with its product detail lines
    /// </summary>
    [HttpGet("operation-logs/{id}")]
    public async Task<ActionResult<ApiResponse<RsdOperationLogWithDetailsDto>>> GetOperationLogById(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetRsdOperationLogByIdQuery(id));
            if (result == null)
                return ErrorResponse<RsdOperationLogWithDetailsDto>("RSD operation log not found", 404);

            return SuccessResponse(result, "RSD operation log retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<RsdOperationLogWithDetailsDto>($"Error retrieving RSD operation log: {ex.Message}", 500);
        }
    }
}