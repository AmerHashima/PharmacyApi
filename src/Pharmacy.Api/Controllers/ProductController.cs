using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Product;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing pharmaceutical products
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class ProductController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get product data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated product data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetProductData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetProductDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Product data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<ProductDto>>($"Error retrieving product data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all products with optional filters
    /// </summary>
    /// <param name="productTypeId">Optional: Filter by product type</param>
    /// <param name="searchTerm">Optional: Search by name or GTIN</param>
    /// <returns>List of products</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProducts(
        [FromQuery] Guid? productTypeId = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetProductListQuery(productTypeId, searchTerm);
        var products = await _mediator.Send(query);
        return SuccessResponse(products, "Products retrieved successfully");
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetProduct(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);

        if (product == null)
            return ErrorResponse<ProductDto>("Product not found", 404);

        return SuccessResponse(product, "Product retrieved successfully");
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="createProductDto">Product creation data</param>
    /// <returns>Created product</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        try
        {
            var command = new CreateProductCommand(createProductDto);
            var product = await _mediator.Send(command);
            return CreatedResponse(product, nameof(GetProduct), new { id = product.Oid }, "Product created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ProductDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="updateProductDto">Product update data</param>
    /// <returns>Updated product</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        try
        {
            if (id != updateProductDto.Oid)
                return ErrorResponse<ProductDto>("Product ID mismatch", 400);

            var command = new UpdateProductCommand(updateProductDto);
            var product = await _mediator.Send(command);
            return SuccessResponse(product, "Product updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<ProductDto>("Product not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ProductDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a product (soft delete)
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteProduct(Guid id)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return ErrorResponse("Product not found", 404);

        return SuccessResponse("Product deleted successfully");
    }
}
