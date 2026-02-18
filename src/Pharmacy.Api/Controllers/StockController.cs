using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.Stock;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Stock;
using Pharmacy.Application.Queries.StockTransaction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing stock and stock transactions
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class StockController : BaseApiController
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Stock Queries

    /// <summary>
    /// Get stock data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated stock data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockDto>>>> GetStockData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetStockDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Stock data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<StockDto>>($"Error retrieving stock data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get stock by branch
    /// </summary>
    /// <param name="branchId">Branch ID</param>
    /// <returns>List of stock items for the branch</returns>
    [HttpGet("branch/{branchId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StockDto>>>> GetStockByBranch(Guid branchId)
    {
        var query = new GetStockByBranchQuery(branchId);
        var stocks = await _mediator.Send(query);
        return SuccessResponse(stocks, "Stock retrieved successfully");
    }

    #endregion

    #region Stock Transactions

    /// <summary>
    /// Get stock transaction data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated stock transaction data</returns>
    [HttpPost("transactions/query")]
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
    /// Get stock transactions with optional filters
    /// </summary>
    /// <param name="branchId">Optional: Filter by branch</param>
    /// <param name="startDate">Optional: Start date filter</param>
    /// <param name="endDate">Optional: End date filter</param>
    /// <param name="transactionTypeId">Optional: Filter by transaction type</param>
    /// <returns>List of stock transactions</returns>
    [HttpGet("transactions")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StockTransactionDto>>>> GetTransactions(
        [FromQuery] Guid? branchId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] Guid? transactionTypeId = null)
    {
        var query = new GetStockTransactionListQuery(branchId, startDate, endDate, transactionTypeId);
        var transactions = await _mediator.Send(query);
        return SuccessResponse(transactions, "Stock transactions retrieved successfully");
    }

    /// <summary>
    /// Create a Stock IN transaction (receive inventory from supplier)
    /// </summary>
    /// <param name="stockInDto">Stock IN data</param>
    /// <returns>Created transaction</returns>
    [HttpPost("in")]
    public async Task<ActionResult<ApiResponse<StockTransactionDto>>> CreateStockIn([FromBody] CreateStockInDto stockInDto)
    {
        try
        {
            var command = new CreateStockInCommand(stockInDto);
            var transaction = await _mediator.Send(command);
            return SuccessResponse(transaction, "Stock IN transaction created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<StockTransactionDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StockTransactionDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Create a Stock Transfer transaction (transfer between branches)
    /// </summary>
    /// <param name="transferDto">Stock transfer data</param>
    /// <returns>Created transaction</returns>
    [HttpPost("transfer")]
    public async Task<ActionResult<ApiResponse<StockTransactionDto>>> CreateStockTransfer([FromBody] CreateStockTransferDto transferDto)
    {
        try
        {
            var command = new CreateStockTransferCommand(transferDto);
            var transaction = await _mediator.Send(command);
            return SuccessResponse(transaction, "Stock transfer transaction created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<StockTransactionDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StockTransactionDto>(ex.Message, 400);
        }
    }

    #endregion
}
