using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Infrastructure.Providers.Salla;

public class SallaProvider : IECommerceProvider
{
    private readonly HttpClient _httpClient;

    public SallaProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri("https://api.salla.dev/");
    }

    public ECommerceProviderType ProviderType => ECommerceProviderType.Salla;

    public Task<ECommerceTokenDto> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Salla OAuth exchange.");
    }

    public Task<ECommerceTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Salla refresh token.");
    }

    public async Task<IReadOnlyList<ECommerceProductDto>> GetProductsAsync(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var products = new List<ECommerceProductDto>();
        var page = 1;

        while (true)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"admin/v2/products?page={page}&per_page=100");
            ApplySallaAuthHeaders(request, accessToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Salla products request failed ({(int)response.StatusCode} {response.ReasonPhrase}): {TrimForException(responseBody)}");
            }

            using var document = JsonDocument.Parse(responseBody);
            var pageProducts = ExtractProductArray(document.RootElement)
                .Select(MapSallaProduct)
                .Where(x => !string.IsNullOrWhiteSpace(x.ExternalProductId))
                .ToList();

            products.AddRange(pageProducts);

            if (pageProducts.Count == 0 || !HasNextPage(document.RootElement, page))
            {
                break;
            }

            page++;
        }

        return products;
    }

    public async Task<ExternalProductPushResultDto> CreateProductAsync(
        string accessToken,
        LocalProductCatalogItemDto product,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "admin/v2/products")
        {
            Content = JsonContent.Create(CreateSallaProductPayload(product))
        };
        ApplySallaAuthHeaders(request, accessToken);

        return await SendProductMutationAsync(request, product, cancellationToken);
    }

    public async Task<ExternalProductPushResultDto> UpdateProductAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        LocalProductCatalogItemDto product,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"admin/v2/products/{externalProductId}")
        {
            Content = JsonContent.Create(CreateSallaProductPayload(product))
        };
        ApplySallaAuthHeaders(request, accessToken);

        return await SendProductMutationAsync(request, product, cancellationToken);
    }

    public Task<IReadOnlyList<ECommerceOrderDto>> GetOrdersAsync(
        string accessToken,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Salla order response mapping.");
    }

    public Task<ECommerceOrderDto?> GetOrderByIdAsync(
        string accessToken,
        string externalOrderId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Salla single order response mapping.");
    }

    public Task UpdateStockAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        decimal quantity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Salla stock sync.");
    }

    public Task UpdatePriceAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        decimal price,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Salla price sync.");
    }

    private async Task<ExternalProductPushResultDto> SendProductMutationAsync(
        HttpRequestMessage request,
        LocalProductCatalogItemDto product,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return new ExternalProductPushResultDto
            {
                Success = false,
                ErrorMessage = $"Salla product push failed ({(int)response.StatusCode} {response.ReasonPhrase}): {TrimForException(responseBody)}",
                RawResponse = responseBody
            };
        }

        using var document = JsonDocument.Parse(responseBody);
        var productElement = ExtractProductObject(document.RootElement);
        var mapped = MapSallaProduct(productElement);

        return new ExternalProductPushResultDto
        {
            Success = true,
            ExternalProductId = mapped.ExternalProductId,
            ExternalVariantId = mapped.ExternalVariantId,
            ExternalSku = mapped.Sku ?? product.SKU,
            ExternalBarcode = mapped.Gtin ?? mapped.Barcode ?? product.GTIN ?? product.Barcode,
            RawResponse = responseBody
        };
    }

    private static object CreateSallaProductPayload(LocalProductCatalogItemDto product)
    {
        return new
        {
            name = product.ProductNameEn ?? product.ProductNameAr,
            price = product.Price,
            sku = product.SKU,
            quantity = product.Quantity,
            barcode = product.GTIN ?? product.Barcode,
            description = product.DescriptionEn ?? product.DescriptionAr ?? product.ProductNameAr,
            status = product.IsActive ? "sale" : "hidden"
        };
    }

    private static ECommerceProductDto MapSallaProduct(JsonElement element)
    {
        var product = ExtractProductObject(element);
        var variant = TryGetFirstArrayItem(product, "options")
            ?? TryGetFirstArrayItem(product, "variants")
            ?? product;

        return new ECommerceProductDto
        {
            ExternalProductId = GetString(product, "id") ?? string.Empty,
            ExternalVariantId = variant.ValueKind == JsonValueKind.Object && !IsSameJsonObject(product, variant)
                ? GetString(variant, "id")
                : null,
            Sku = GetString(variant, "sku") ?? GetString(product, "sku"),
            Barcode = GetString(variant, "barcode") ?? GetString(product, "barcode"),
            Gtin = GetString(variant, "gtin") ?? GetString(product, "gtin"),
            ProductName = GetString(product, "name") ?? GetString(product, "title"),
            Price = GetDecimal(variant, "price") ?? GetDecimal(product, "price"),
            StockQuantity = GetDecimal(variant, "quantity")
                ?? GetDecimal(product, "quantity")
                ?? GetDecimal(product, "stock_quantity")
        };
    }

    private static IReadOnlyList<JsonElement> ExtractProductArray(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array)
        {
            return root.EnumerateArray().ToList();
        }

        foreach (var propertyName in new[] { "data", "products", "items", "results" })
        {
            if (TryGetProperty(root, propertyName, out var property) && property.ValueKind == JsonValueKind.Array)
            {
                return property.EnumerateArray().ToList();
            }
        }

        return Array.Empty<JsonElement>();
    }

    private static JsonElement ExtractProductObject(JsonElement root)
    {
        foreach (var propertyName in new[] { "data", "product", "item" })
        {
            if (TryGetProperty(root, propertyName, out var property) && property.ValueKind == JsonValueKind.Object)
            {
                return property;
            }
        }

        return root;
    }

    private static JsonElement? TryGetFirstArrayItem(JsonElement element, string propertyName)
    {
        if (!TryGetProperty(element, propertyName, out var property) || property.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        return property.EnumerateArray().FirstOrDefault();
    }

    private static bool HasNextPage(JsonElement root, int page)
    {
        if (TryGetProperty(root, "pagination", out var pagination))
        {
            var currentPage = GetInt(pagination, "currentPage")
                ?? GetInt(pagination, "current_page")
                ?? page;
            var totalPages = GetInt(pagination, "totalPages")
                ?? GetInt(pagination, "total_pages");

            return totalPages.HasValue && currentPage < totalPages.Value;
        }

        return false;
    }

    private static void ApplySallaAuthHeaders(HttpRequestMessage request, string accessToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        if (!TryGetProperty(element, propertyName, out var property))
        {
            return null;
        }

        return property.ValueKind switch
        {
            JsonValueKind.String => property.GetString(),
            JsonValueKind.Number => property.GetRawText(),
            JsonValueKind.True => bool.TrueString,
            JsonValueKind.False => bool.FalseString,
            _ => null
        };
    }

    private static decimal? GetDecimal(JsonElement element, string propertyName)
    {
        if (!TryGetProperty(element, propertyName, out var property))
        {
            return null;
        }

        if (property.ValueKind == JsonValueKind.Number && property.TryGetDecimal(out var value))
        {
            return value;
        }

        return property.ValueKind == JsonValueKind.String && decimal.TryParse(property.GetString(), out value)
            ? value
            : null;
    }

    private static int? GetInt(JsonElement element, string propertyName)
    {
        if (!TryGetProperty(element, propertyName, out var property))
        {
            return null;
        }

        if (property.ValueKind == JsonValueKind.Number && property.TryGetInt32(out var value))
        {
            return value;
        }

        return property.ValueKind == JsonValueKind.String && int.TryParse(property.GetString(), out value)
            ? value
            : null;
    }

    private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement property)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            property = default;
            return false;
        }

        foreach (var item in element.EnumerateObject())
        {
            if (string.Equals(item.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                property = item.Value;
                return true;
            }
        }

        property = default;
        return false;
    }

    private static bool IsSameJsonObject(JsonElement left, JsonElement right)
    {
        return left.ValueKind == JsonValueKind.Object
               && right.ValueKind == JsonValueKind.Object
               && left.GetRawText() == right.GetRawText();
    }

    private static string TrimForException(string value)
    {
        const int maxLength = 1000;
        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
