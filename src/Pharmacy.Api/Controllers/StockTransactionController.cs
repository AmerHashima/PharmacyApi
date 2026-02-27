using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.Queries.StockTransaction;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing stock transactions (master/header with details)
/// Handles IN, OUT, TRANSFER, and other stock movements
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class StockTransactionController : BaseApiController
{
    private readonly IMediator _mediator;

    public StockTransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get stock transaction by ID with all detail lines
    /// </summary>
    /// <param name="id">Transaction ID</param>
    /// <returns>Transaction with details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StockTransactionWithDetailsDto>>> GetTransaction(Guid id)
    {
        var query = new GetStockTransactionByIdQuery(id);
        var transaction = await _mediator.Send(query);

        if (transaction == null)
            return ErrorResponse<StockTransactionWithDetailsDto>("Stock transaction not found", 404);

        return SuccessResponse(transaction, "Stock transaction retrieved successfully");
    }

    /// <summary>
    /// Get stock transactions with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated transaction data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockTransactionDto>>>> GetTransactionData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetStockTransactionDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Stock transaction data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<StockTransactionDto>>($"Error retrieving transaction data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Create a new stock transaction with detail lines
    /// </summary>
    /// <param name="createDto">Transaction creation data with details</param>
    /// <returns>Created transaction with details</returns>
    /// <remarks>
    /// Example request:
    /// 
    ///     POST /api/StockTransaction
    ///     {
    ///       "transactionTypeId": "transaction-type-guid",
    ///       "fromBranchId": "branch-1-guid",
    ///       "toBranchId": "branch-2-guid",
    ///       "transactionDate": "2026-02-27T12:00:00Z",
    ///       "referenceNumber": "TRF-2026-001",
    ///       "status": "Draft",
    ///       "notes": "Stock transfer",
    ///       "details": [
    ///         {
    ///           "productId": "product-1-guid",
    ///           "quantity": 100,
    ///           "unitCost": 5.00,
    ///           "batchNumber": "BATCH-001",
    ///           "expiryDate": "2028-02-27",
    ///           "lineNumber": 1
    ///         }
    ///       ]
    ///     }
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<StockTransactionWithDetailsDto>>> CreateTransaction(
        [FromBody] CreateStockTransactionWithDetailsDto createDto)
    {
        try
        {
            var command = new CreateStockTransactionWithDetailsCommand(createDto);
            var transaction = await _mediator.Send(command);
            return CreatedResponse(
                transaction, 
                nameof(GetTransaction), 
                new { id = transaction.Oid }, 
                "Stock transaction created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<StockTransactionWithDetailsDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StockTransactionWithDetailsDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing stock transaction with detail lines
    /// </summary>
    /// <param name="id">Transaction ID</param>
    /// <param name="updateDto">Transaction update data with details</param>
    /// <returns>Updated transaction with details</returns>
    /// <remarks>
    /// This replaces all existing detail lines with the new ones provided.
    /// To add/remove individual lines, use the StockTransactionDetail endpoints.
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StockTransactionWithDetailsDto>>> UpdateTransaction(
        Guid id, 
        [FromBody] UpdateStockTransactionWithDetailsDto updateDto)
    {
        try
        {
            if (id != updateDto.Oid)
                return ErrorResponse<StockTransactionWithDetailsDto>("Transaction ID mismatch", 400);

            var command = new UpdateStockTransactionWithDetailsCommand(updateDto);
            var transaction = await _mediator.Send(command);
            return SuccessResponse(transaction, "Stock transaction updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<StockTransactionWithDetailsDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StockTransactionWithDetailsDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a stock transaction (soft delete, including all detail lines)
    /// </summary>
    /// <param name="id">Transaction ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteTransaction(Guid id)
    {
        var command = new DeleteStockTransactionCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return ErrorResponse("Stock transaction not found", 404);

        return SuccessResponse("Stock transaction deleted successfully");
    }

    /// <summary>
    /// Approve a stock transaction (changes status to Approved)
    /// </summary>
    /// <param name="id">Transaction ID</param>
    /// <returns>Approved transaction</returns>
    [HttpPost("{id}/approve")]
    public async Task<ActionResult<ApiResponse<StockTransactionWithDetailsDto>>> ApproveTransaction(Guid id)
    {
        // TODO: Implement approval logic
        // This would:
        // 1. Get transaction
        // 2. Validate it's in Draft status
        // 3. Set Status = "Approved"
        // 4. Set ApprovedBy = current user
        // 5. Set ApprovedDate = now
        // 6. Update stock quantities if applicable
        return ErrorResponse<StockTransactionWithDetailsDto>("Not implemented yet", 501);
    }

    /// <summary>
    /// Cancel a stock transaction
    /// </summary>
    /// <param name="id">Transaction ID</param>
    /// <returns>Cancelled transaction</returns>
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ApiResponse<StockTransactionWithDetailsDto>>> CancelTransaction(Guid id)
    {
        // TODO: Implement cancellation logic
        return ErrorResponse<StockTransactionWithDetailsDto>("Not implemented yet", 501);
    }
}
