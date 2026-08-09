using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace ECommerceIntegration.Infrastructure.Providers.Zid;

public class ZidProvider : IECommerceProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ZidProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress ??= new Uri("https://api.zid.sa/");
    }

    public ECommerceProviderType ProviderType => ECommerceProviderType.Zid;

    public async Task<ECommerceTokenDto> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    {
        var section = _configuration.GetSection("ECommerceIntegration:Zid");
        var clientId = section["ClientId"];
        var clientSecret = section["ClientSecret"];
        var redirectUri = section["RedirectUri"];
        var configuredAccessToken = section["AccessToken"]
            ?? section["Authorization"]
            ?? section["AuthorizationToken"];
        var configuredStoreId = section["StoreId"];
        var tokenUrl = section["TokenUrl"] ?? "https://oauth.zid.sa/oauth/token";

        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidOperationException("Zid ClientId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new InvalidOperationException("Zid ClientSecret is not configured.");
        }

        if (string.IsNullOrWhiteSpace(redirectUri))
        {
            throw new InvalidOperationException("Zid RedirectUri is not configured.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["redirect_uri"] = redirectUri,
                ["code"] = code
            })
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Zid OAuth token exchange failed ({(int)response.StatusCode} {response.ReasonPhrase}): {TrimForException(responseBody)}");
        }

        using var tokenJson = JsonDocument.Parse(responseBody);
        var root = tokenJson.RootElement;
        var accessToken = GetString(root, "access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException("Zid OAuth token response did not include access_token.");
        }

        var authorization = GetString(root, "authorization")
            ?? GetString(root, "Authorization")
            ?? configuredAccessToken;

        var storeId = ExtractJwtSubject(authorization)
            ?? configuredStoreId
            ?? GetString(root, "store_id")
            ?? GetString(root, "store_uuid")
            ?? GetString(root, "merchant_id");

        var token = new ECommerceTokenDto
        {
            AccessToken = BuildStoredAccessToken(authorization, accessToken, storeId),
            RefreshToken = GetString(root, "refresh_token"),
            ExpiresAt = GetExpiresAt(root)
        };

        ApplyStoreDetailsFromTokenResponse(root, token);

        if (!string.IsNullOrWhiteSpace(authorization))
        {
            await ApplyStoreDetailsFromStoreApiAsync(authorization, accessToken, token, cancellationToken);
        }

        token.ExternalStoreId ??= storeId ?? $"zid-token:{HashToken(accessToken)}";
        token.StoreName ??= "Zid Store";

        return token;
    }

    public Task<ECommerceTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Zid refresh token.");
    }

    public async Task<IReadOnlyList<ECommerceProductDto>> GetProductsAsync(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var products = new List<ECommerceProductDto>();
        var page = 1;

        while (true)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"v1/products/?page={page}&page_size=100");
            ApplyZidAuthHeaders(request, accessToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Zid products request failed ({(int)response.StatusCode} {response.ReasonPhrase}): {TrimForException(responseBody)}");
            }

            using var document = JsonDocument.Parse(responseBody);
            var pageProducts = ExtractProductArray(document.RootElement)
                .Select(MapZidProduct)
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
        using var request = new HttpRequestMessage(HttpMethod.Post, "v1/products/")
        {
            Content = JsonContent.Create(CreateZidProductPayload(product))
        };
        ApplyZidAuthHeaders(request, accessToken);

        return await SendProductMutationAsync(request, product, cancellationToken);
    }

    public async Task<ExternalProductPushResultDto> UpdateProductAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        LocalProductCatalogItemDto product,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"v1/products/{externalProductId}/")
        {
            Content = JsonContent.Create(CreateZidProductPayload(product))
        };
        ApplyZidAuthHeaders(request, accessToken);

        return await SendProductMutationAsync(request, product, cancellationToken);
    }

    public async Task<IReadOnlyList<ECommerceOrderDto>> GetOrdersAsync(
        string accessToken,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"v1/orders/?from={fromDate:yyyy-MM-dd}&to={toDate:yyyy-MM-dd}");
        ApplyZidAuthHeaders(request, accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
        return new List<ECommerceOrderDto>();
    }

    public Task<ECommerceOrderDto?> GetOrderByIdAsync(
        string accessToken,
        string externalOrderId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Zid single order response mapping.");
    }

    public Task UpdateStockAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        decimal quantity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Zid stock sync.");
    }

    public Task UpdatePriceAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        decimal price,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Implement Zid price sync.");
    }

    private async Task ApplyStoreDetailsFromStoreApiAsync(
        string authorization,
        string managerToken,
        ECommerceTokenDto token,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "v1/managers/account/store/");
        request.Headers.TryAddWithoutValidation("Authorization", EnsureBearerToken(authorization));
        request.Headers.TryAddWithoutValidation("X-Manager-Token", managerToken);
        request.Headers.TryAddWithoutValidation("Accept-Language", "en");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        using var storeJson = JsonDocument.Parse(responseBody);
        if (!TryGetProperty(storeJson.RootElement, "store", out var store))
        {
            return;
        }

        token.ExternalStoreId = GetString(store, "uuid")
            ?? GetString(store, "id")
            ?? token.ExternalStoreId;
        token.StoreName = GetString(store, "title")
            ?? GetString(store, "name")
            ?? token.StoreName;
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
                ErrorMessage = $"Zid product push failed ({(int)response.StatusCode} {response.ReasonPhrase}): {TrimForException(responseBody)}",
                RawResponse = responseBody
            };
        }

        using var document = JsonDocument.Parse(responseBody);
        var productElement = ExtractProductObject(document.RootElement);
        var mapped = MapZidProduct(productElement);

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

    private static object CreateZidProductPayload(LocalProductCatalogItemDto product)
    {
        // Zid product schemas differ by app permissions/version; keep this payload conservative.
        return new
        {
            name = product.ProductNameAr,
            name_ar = product.ProductNameAr,
            name_en = product.ProductNameEn ?? product.ProductNameAr,
            price = product.Price,
            sku = product.SKU,
            barcode = product.GTIN ?? product.Barcode,
            quantity = product.Quantity,
            is_published = product.IsActive,
            description = product.DescriptionAr ?? product.DescriptionEn ?? product.ProductNameAr
        };
    }

    private static ECommerceProductDto MapZidProduct(JsonElement element)
    {
        var product = ExtractProductObject(element);
        var variant = TryGetFirstArrayItem(product, "variants")
            ?? TryGetFirstArrayItem(product, "options")
            ?? product;

        return new ECommerceProductDto
        {
            ExternalProductId = GetString(product, "id")
                ?? GetString(product, "uuid")
                ?? GetString(product, "product_id")
                ?? string.Empty,
            ExternalVariantId = variant.ValueKind == JsonValueKind.Object && !IsSameJsonObject(product, variant)
                ? GetString(variant, "id") ?? GetString(variant, "uuid")
                : null,
            Sku = GetString(variant, "sku") ?? GetString(product, "sku"),
            Barcode = GetString(variant, "barcode") ?? GetString(product, "barcode"),
            Gtin = GetString(variant, "gtin") ?? GetString(product, "gtin"),
            ProductName = GetString(product, "name")
                ?? GetString(product, "title")
                ?? GetString(product, "name_ar")
                ?? GetString(product, "name_en"),
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

        foreach (var propertyName in new[] { "products", "data", "items", "results" })
        {
            if (TryGetProperty(root, propertyName, out var property))
            {
                if (property.ValueKind == JsonValueKind.Array)
                {
                    return property.EnumerateArray().ToList();
                }

                if (TryGetProperty(property, "data", out var nested) && nested.ValueKind == JsonValueKind.Array)
                {
                    return nested.EnumerateArray().ToList();
                }
            }
        }

        return Array.Empty<JsonElement>();
    }

    private static JsonElement ExtractProductObject(JsonElement root)
    {
        foreach (var propertyName in new[] { "product", "data", "item" })
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
        var currentPage = GetInt(root, "current_page") ?? GetInt(root, "page") ?? page;
        var lastPage = GetInt(root, "last_page") ?? GetInt(root, "total_pages");

        if (lastPage.HasValue)
        {
            return currentPage < lastPage.Value;
        }

        if (TryGetProperty(root, "links", out var links)
            && (GetString(links, "next") != null || GetString(root, "next") != null))
        {
            return true;
        }

        return false;
    }

    private static void ApplyZidAuthHeaders(HttpRequestMessage request, string accessToken)
    {
        var parsed = ParseStoredAccessToken(accessToken);
        var apiAccessToken = parsed.Authorization ?? accessToken;
        var storeId = parsed.StoreId ?? ExtractJwtSubject(apiAccessToken);

        request.Headers.TryAddWithoutValidation("Access-Token", RemoveBearerPrefix(apiAccessToken));
        request.Headers.TryAddWithoutValidation("Accept-Language", "en");
        request.Headers.TryAddWithoutValidation("Role", "Manager");
        if (!string.IsNullOrWhiteSpace(storeId))
        {
            request.Headers.TryAddWithoutValidation("Store-Id", storeId);
        }

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private static void ApplyStoreDetailsFromTokenResponse(JsonElement root, ECommerceTokenDto token)
    {
        token.ExternalStoreId = GetString(root, "store_uuid")
            ?? GetString(root, "store_id")
            ?? GetString(root, "merchant_id")
            ?? token.ExternalStoreId;
        token.StoreName = GetString(root, "store_name")
            ?? GetString(root, "store_title")
            ?? token.StoreName;

        if (TryGetProperty(root, "store", out var store))
        {
            token.ExternalStoreId = GetString(store, "uuid")
                ?? GetString(store, "id")
                ?? token.ExternalStoreId;
            token.StoreName = GetString(store, "title")
                ?? GetString(store, "name")
                ?? token.StoreName;
        }
    }

    private static DateTime? GetExpiresAt(JsonElement root)
    {
        if (!TryGetProperty(root, "expires_in", out var expiresIn))
        {
            return null;
        }

        if (expiresIn.ValueKind == JsonValueKind.Number && expiresIn.TryGetInt64(out var seconds))
        {
            return DateTime.UtcNow.AddSeconds(seconds);
        }

        if (expiresIn.ValueKind == JsonValueKind.String)
        {
            var value = expiresIn.GetString();
            if (long.TryParse(value, out seconds))
            {
                return DateTime.UtcNow.AddSeconds(seconds);
            }

            if (DateTime.TryParse(value, out var expiresAt))
            {
                return expiresAt.ToUniversalTime();
            }
        }

        return null;
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

    private static string EnsureBearerToken(string token)
    {
        return token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? token
            : $"Bearer {token}";
    }

    private static string BuildStoredAccessToken(string? authorization, string managerToken, string? storeId)
    {
        if (string.IsNullOrWhiteSpace(authorization))
        {
            return managerToken;
        }

        return JsonSerializer.Serialize(new StoredZidToken(authorization, managerToken, storeId));
    }

    private static StoredZidToken ParseStoredAccessToken(string accessToken)
    {
        if (!accessToken.TrimStart().StartsWith("{", StringComparison.Ordinal))
        {
            return new StoredZidToken(null, accessToken, null);
        }

        try
        {
            return JsonSerializer.Deserialize<StoredZidToken>(accessToken)
                ?? new StoredZidToken(null, accessToken, null);
        }
        catch (JsonException)
        {
            return new StoredZidToken(null, accessToken, null);
        }
    }

    private static string RemoveBearerPrefix(string token)
    {
        return token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? token["Bearer ".Length..]
            : token;
    }

    private static string? ExtractJwtSubject(string? jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
        {
            return null;
        }

        var token = RemoveBearerPrefix(jwt);
        var parts = token.Split('.');
        if (parts.Length < 2)
        {
            return null;
        }

        try
        {
            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');
            payload = payload.PadRight(payload.Length + ((4 - payload.Length % 4) % 4), '=');

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
            using var document = JsonDocument.Parse(json);
            return GetString(document.RootElement, "sub");
        }
        catch (Exception ex) when (ex is FormatException or JsonException or ArgumentException)
        {
            return null;
        }
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

    private static string HashToken(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hash)[..32].ToLowerInvariant();
    }

    private sealed record StoredZidToken(string? Authorization, string? ManagerToken, string? StoreId);
}
