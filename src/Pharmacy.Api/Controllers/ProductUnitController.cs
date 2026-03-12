using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.ProductUnit;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.ProductUnit;
using Pharmacy.Application.Queries.ProductUnit;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing product packaging units and conversion factors
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class ProductUnitController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductUnitController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Query product units with advanced filtering, sorting, and pagination
    /// </summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<ProductUnitDto>>>> GetProductUnitData([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetProductUnitDataQuery(request));
            return SuccessResponse(result, "Product unit data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<ProductUnitDto>>($"Error retrieving product unit data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all units for a specific product
    /// </summary>
    [HttpGet("by-product/{productId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductUnitDto>>>> GetByProduct(Guid productId)
    {
        var result = await _mediator.Send(new GetProductUnitsByProductIdQuery(productId));
        return SuccessResponse(result, "Product units retrieved successfully");
    }

    /// <summary>
    /// Get product unit by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductUnitDto>>> GetProductUnit(Guid id)
    {
        var result = await _mediator.Send(new GetProductUnitByIdQuery(id));
        if (result == null)
            return ErrorResponse<ProductUnitDto>("Product unit not found", 404);

        return SuccessResponse(result, "Product unit retrieved successfully");
    }

    /// <summary>
    /// Create a new product unit
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductUnitDto>>> CreateProductUnit([FromBody] CreateProductUnitDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateProductUnitCommand(dto));
            return CreatedResponse(result, nameof(GetProductUnit), new { id = result.Oid }, "Product unit created successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<ProductUnitDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<ProductUnitDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Update an existing product unit
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductUnitDto>>> UpdateProductUnit(Guid id, [FromBody] UpdateProductUnitDto dto)
    {
        try
        {
            if (id != dto.Oid)
                return ErrorResponse<ProductUnitDto>("Product unit ID mismatch", 400);

            var result = await _mediator.Send(new UpdateProductUnitCommand(dto));
            return SuccessResponse(result, "Product unit updated successfully");
        }
        catch (KeyNotFoundException) { return ErrorResponse<ProductUnitDto>("Product unit not found", 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<ProductUnitDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Delete a product unit (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteProductUnit(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductUnitCommand(id));
        if (!result)
            return ErrorResponse("Product unit not found", 404);

        return SuccessResponse("Product unit deleted successfully");
    }
}
