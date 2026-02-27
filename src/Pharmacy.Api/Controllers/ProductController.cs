using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Product;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Product;
using Pharmacy.Application.Services;
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
    private readonly IBarcodeParserService _barcodeParser;

    public ProductController(IMediator mediator, IBarcodeParserService barcodeParser)
    {
        _mediator = mediator;
        _barcodeParser = barcodeParser;
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

    /// <summary>
    /// Parse GS1 barcode (DataMatrix/QR code) from pharmaceutical product packaging
    /// Extracts: GTIN, Serial Number, Batch Number, Expiry Date, Production Date
    /// </summary>
    /// <param name="request">Barcode parse request containing the raw barcode string</param>
    /// <returns>Parsed barcode data</returns>
    /// <remarks>
    /// Supports various GS1 Application Identifier (AI) formats:
    /// - 01: GTIN (Global Trade Item Number)
    /// - 21: Serial Number
    /// - 17: Expiry Date (YYMMDD)
    /// - 10: Batch/Lot Number
    /// - 11: Production Date (YYMMDD)
    /// 
    /// Example barcode: 0106285111001038173001261015246521CA6D3NDJ4LJ62
    /// </remarks>
    [HttpPost("parse-barcode")]
    [AllowAnonymous]
    public ActionResult<ApiResponse<BarcodeParseResponseDto>> ParseBarcode([FromBody] BarcodeParseRequestDto request)
    {
        try
        {
            var result = _barcodeParser.ParseBarcode(request.BarcodeInput);

            if (!result.Success)
                return ErrorResponse<BarcodeParseResponseDto>(result.ErrorMessage ?? "Failed to parse barcode", 400);

            return SuccessResponse(result, "Barcode parsed successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<BarcodeParseResponseDto>($"Error parsing barcode: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Parse barcode and retrieve product by GTIN in one operation
    /// </summary>
    /// <param name="request">Barcode parse request containing the raw barcode string</param>
    /// <returns>Parsed barcode data with associated product details</returns>
    /// <remarks>
    /// This endpoint:
    /// 1. Parses the GS1 barcode to extract GTIN and other data
    /// 2. Looks up the product in the database using the extracted GTIN
    /// 3. Returns both the parsed barcode data and product details
    /// 
    /// Example barcode: 0106285111001038173001261015246521CA6D3NDJ4LJ62
    /// </remarks>
    [HttpPost("parse-and-get-product")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<BarcodeProductResponseDto>>> ParseBarcodeAndGetProduct([FromBody] BarcodeParseRequestDto request)
    {
        try
        {
            // Step 1: Parse the barcode
            var barcodeResult = _barcodeParser.ParseBarcode(request.BarcodeInput);

            var response = new BarcodeProductResponseDto
            {
                BarcodeData = barcodeResult
            };

            if (!barcodeResult.Success)
            {
                response.ProductFound = false;
                response.ProductMessage = "Barcode parsing failed";
                return ErrorResponse<BarcodeProductResponseDto>(
                    barcodeResult.ErrorMessage ?? "Failed to parse barcode", 400);
            }

            // Step 2: Get product by GTIN if available
            if (!string.IsNullOrEmpty(barcodeResult.GTIN))
            {
                var query = new GetProductByGTINQuery(barcodeResult.GTIN);
                var product = await _mediator.Send(query);

                if (product != null)
                {
                    response.Product = product;
                    response.ProductFound = true;
                    response.ProductMessage = "Product found successfully";
                    return SuccessResponse(response, "Barcode parsed and product retrieved successfully");
                }
                else
                {
                    response.ProductFound = false;
                    response.ProductMessage = $"Product with GTIN '{barcodeResult.GTIN}' not found in database";
                    return SuccessResponse(response, "Barcode parsed successfully, but product not found");
                }
            }
            else
            {
                response.ProductFound = false;
                response.ProductMessage = "GTIN not found in barcode";
                return ErrorResponse<BarcodeProductResponseDto>("GTIN not found in parsed barcode data", 400);
            }
        }
        catch (Exception ex)
        {
            return ErrorResponse<BarcodeProductResponseDto>($"Error processing barcode: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get product by GTIN
    /// </summary>
    /// <param name="gtin">Global Trade Item Number (14 digits)</param>
    /// <returns>Product details</returns>
    [HttpGet("gtin/{gtin}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductByGTIN(string gtin)
    {
        try
        {
            var query = new GetProductByGTINQuery(gtin);
            var product = await _mediator.Send(query);

            if (product == null)
                return ErrorResponse<ProductDto>($"Product with GTIN '{gtin}' not found", 404);

            return SuccessResponse(product, "Product retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<ProductDto>($"Error retrieving product: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get multiple products by list of GTINs (batch lookup)
    /// </summary>
    /// <param name="gtins">List of GTINs to search for</param>
    /// <returns>List of products matching the GTINs</returns>
    /// <remarks>
    /// Example request body:
    /// ["6281086011508", "06820034800001", "06820034800002"]
    /// </remarks>
    [HttpPost("gtin/batch")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProductsByGTINs([FromBody] List<string> gtins)
    {
        try
        {
            if (gtins == null || !gtins.Any())
                return ErrorResponse<IEnumerable<ProductDto>>("GTIN list is required", 400);

            var query = new GetProductsByGTINsQuery(gtins);
            var products = await _mediator.Send(query);

            var productList = products.ToList();
            var message = productList.Any() 
                ? $"Found {productList.Count} product(s) out of {gtins.Count} GTIN(s)" 
                : "No products found for the provided GTINs";

            return SuccessResponse<IEnumerable<ProductDto>>(productList, message);
        }
        catch (Exception ex)
        {
            return ErrorResponse<IEnumerable<ProductDto>>($"Error retrieving products: {ex.Message}", 500);
        }
    }
}
