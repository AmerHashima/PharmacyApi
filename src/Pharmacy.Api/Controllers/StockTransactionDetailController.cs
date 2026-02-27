using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.Queries.StockTransaction;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing stock transaction details (line items)
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class StockTransactionDetailController : BaseApiController
{
    private readonly IMediator _mediator;

    public StockTransactionDetailController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get stock transaction detail by ID
    /// </summary>
    /// <param name="id">Detail ID</param>
    /// <returns>Detail information</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StockTransactionDetailDto>>> GetDetail(Guid id)
    {
        var query = new GetStockTransactionDetailByIdQuery(id);
        var detail = await _mediator.Send(query);

        if (detail == null)
            return ErrorResponse<StockTransactionDetailDto>("Stock transaction detail not found", 404);

        return SuccessResponse(detail, "Stock transaction detail retrieved successfully");
    }

    /// <summary>
    /// Get all details for a specific stock transaction
    /// </summary>
    /// <param name="transactionId">Stock transaction ID</param>
    /// <returns>List of detail lines</returns>
    [HttpGet("by-transaction/{transactionId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StockTransactionDetailDto>>>> GetDetailsByTransaction(Guid transactionId)
    {
        var query = new GetStockTransactionDetailsByTransactionIdQuery(transactionId);
        var details = await _mediator.Send(query);

        return SuccessResponse(details, "Stock transaction details retrieved successfully");
    }

    /// <summary>
    /// Create a new stock transaction detail
    /// </summary>
    /// <param name="createDto">Detail creation data</param>
    /// <returns>Created detail</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<StockTransactionDetailDto>>> CreateDetail([FromBody] CreateStockTransactionDetailDto createDto)
    {
        try
        {
            var command = new CreateStockTransactionDetailCommand(createDto);
            var detail = await _mediator.Send(command);
            return CreatedResponse(detail, nameof(GetDetail), new { id = detail.Oid }, "Stock transaction detail created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<StockTransactionDetailDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StockTransactionDetailDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing stock transaction detail
    /// </summary>
    /// <param name="id">Detail ID</param>
    /// <param name="updateDto">Detail update data</param>
    /// <returns>Updated detail</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StockTransactionDetailDto>>> UpdateDetail(Guid id, [FromBody] UpdateStockTransactionDetailDto updateDto)
    {
        try
        {
            if (id != updateDto.Oid)
                return ErrorResponse<StockTransactionDetailDto>("Detail ID mismatch", 400);

            var command = new UpdateStockTransactionDetailCommand(updateDto);
            var detail = await _mediator.Send(command);
            return SuccessResponse(detail, "Stock transaction detail updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<StockTransactionDetailDto>("Stock transaction detail not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StockTransactionDetailDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a stock transaction detail (soft delete)
    /// </summary>
    /// <param name="id">Detail ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteDetail(Guid id)
    {
        var command = new DeleteStockTransactionDetailCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return ErrorResponse("Stock transaction detail not found", 404);

        return SuccessResponse("Stock transaction detail deleted successfully");
    }
}
