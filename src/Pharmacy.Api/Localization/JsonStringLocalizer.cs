using System.Collections.Concurrent;
using System.Text.Json;

namespace Pharmacy.Api.Localization;

public static class JsonStringLocalizer
{
    private static readonly ConcurrentDictionary<string, Dictionary<string, string>> _cache = new();

    public static Dictionary<string, string> Load(string culture)
    {
        if (_cache.TryGetValue(culture, out var dict)) return dict;

        var basePath = Path.Combine(AppContext.BaseDirectory, "Resources");
        var file = Path.Combine(basePath, culture + ".json");
        if (!File.Exists(file))
        {
            // fallback to en
            file = Path.Combine(basePath, "en.json");
        }

        var json = File.ReadAllText(file);
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        _cache[culture] = data;
        return data;
    }

    public static string? Get(string key, string culture = "en")
    {
        var dict = Load(culture);
        if (dict.TryGetValue(key, out var val)) return val;
        // fallback to en
        if (culture != "en")
        {
            var en = Load("en");
            if (en.TryGetValue(key, out var v2)) return v2;
        }
        return null;
    }

    public static string? FindKeyForValue(string value, string culture = "en")
    {
        if (string.IsNullOrEmpty(value)) return null;
        var dict = Load(culture);
        foreach (var kv in dict)
        {
            if (string.Equals(kv.Value, value, StringComparison.OrdinalIgnoreCase)) return kv.Key;
        }
        return null;
    }
}
