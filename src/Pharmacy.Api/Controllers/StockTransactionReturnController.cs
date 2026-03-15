using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.StockTransactionReturn;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Application.Queries.StockTransactionReturn;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing stock transaction returns (master/header with details)
/// Handles return stock movements linked to return invoices
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class StockTransactionReturnController : BaseApiController
{
    private readonly IMediator _mediator;

    public StockTransactionReturnController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get stock transaction return by ID with all detail lines
    /// </summary>
    /// <param name="id">Return transaction ID</param>
    /// <returns>Return transaction with details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StockTransactionReturnWithDetailsDto>>> GetReturnTransaction(Guid id)
    {
        var query = new GetStockTransactionReturnByIdQuery(id);
        var transaction = await _mediator.Send(query);

        if (transaction == null)
            return ErrorResponse<StockTransactionReturnWithDetailsDto>("Stock transaction return not found", 404);

        return SuccessResponse(transaction, "Stock transaction return retrieved successfully");
    }

    /// <summary>
    /// Get stock transaction returns with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated return transaction data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockTransactionReturnDto>>>> GetReturnTransactionData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetStockTransactionReturnDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Stock transaction return data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<StockTransactionReturnDto>>($"Error retrieving return transaction data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Create a new stock transaction return with detail lines
    /// </summary>
    /// <param name="createDto">Return transaction creation data with details</param>
    /// <returns>Created return transaction with details</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<StockTransactionReturnWithDetailsDto>>> CreateReturnTransaction(
        [FromBody] CreateStockTransactionReturnWithDetailsDto createDto)
    {
        try
        {
            var command = new CreateStockTransactionReturnWithDetailsCommand(createDto);
            var transaction = await _mediator.Send(command);
            return CreatedResponse(
                transaction,
                nameof(GetReturnTransaction),
                new { id = transaction.Oid },
                "Stock transaction return created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<StockTransactionReturnWithDetailsDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StockTransactionReturnWithDetailsDto>(ex.Message, 400);
        }
    }
}
