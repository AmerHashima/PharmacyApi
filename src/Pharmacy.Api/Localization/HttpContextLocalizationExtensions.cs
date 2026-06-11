using Microsoft.AspNetCore.Http;

namespace Pharmacy.Api.Localization;

public static class HttpContextLocalizationExtensions
{
    public static string GetRequestCulture(this HttpContext ctx)
    {
        if (ctx.Request.Headers.TryGetValue("Accept-Language", out var lang) && !string.IsNullOrWhiteSpace(lang))
        {
            var first = lang.ToString().Split(',').FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(first))
            {
                // normalize to 'en' or 'ar' if startsWith
                if (first.StartsWith("ar", StringComparison.OrdinalIgnoreCase)) return "ar";
                return "en";
            }
        }
        return "en";
    }
}
