using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace jaytwo.UrlHelper;

public static class QueryString
{
    public static string Serialize(object data)
    {
        var runtimeProperties = data.GetType().GetRuntimeProperties();
        var dictionary = runtimeProperties
            .Where(x => x.GetValue(data) != null)
            .ToDictionary(m => m.Name, m => m.GetValue(data));

        return SerializeDictionary(dictionary);
    }

    public static string Serialize(IDictionary<string, string?> data) => SerializeDictionary(data);

    public static string Serialize(IDictionary<string, string?[]> data) => SerializeDictionary(data);

    public static string Serialize(IDictionary<string, object?> data) => SerializeDictionary(data);

    public static string Serialize(IDictionary<string, object?[]> data) => SerializeDictionary(data);

    public static string Serialize(IEnumerable<KeyValuePair<string, string?>> data)
    {
        return string.Join("&", data.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value ?? string.Empty)}"));
    }

    public static string Serialize(NameValueCollection data)
    {
        var asDictionary = data.AllKeys.ToDictionary(x => x!, x => data.GetValues(x));
        return SerializeDictionary(asDictionary);
    }

    public static IDictionary<string, string?[]> Deserialize(string queryString)
    {
        var keyValuePairsWithDuplicateKeys = ParseQueryStringAsKeyValuePairs(queryString);

        var result = keyValuePairsWithDuplicateKeys
            .GroupBy(x => x.Key)
            .Select(group =>
            {
                var values = group.Select(x => x.Value).ToArray();
                return new { group.Key, values };
            })
            .ToDictionary(x => x.Key, x => x.values);

        return result;
    }

    private static IList<KeyValuePair<string, string?>> ParseQueryStringAsKeyValuePairs(string queryString)
    {
        var result = new List<KeyValuePair<string, string?>>();

        if (!string.IsNullOrWhiteSpace(queryString))
        {
            queryString = queryString.TrimStart('?');

            var keyValuePairs = queryString.Split('&');
            foreach (var keyValuePair in keyValuePairs)
            {
                var keyValueSplit = keyValuePair.Split('=');
                var key = Uri.UnescapeDataString(keyValueSplit[0]);
                var value = (keyValueSplit.Length > 1) ? Uri.UnescapeDataString(keyValueSplit[1] ?? string.Empty) : null;
                result.Add(new KeyValuePair<string, string?>(key, value));
            }
        }

        return result;
    }

    private static string SerializeDictionary<T>(IDictionary<string, T> data)
    {
        var asKeyValuePairs = GetKeyValuePairs<T>(data).ToList();
        return Serialize(asKeyValuePairs);
    }

    private static IEnumerable<KeyValuePair<string, string?>> GetKeyValuePairs<T>(IDictionary<string, T> data)
    {
        foreach (var keyValuePair in data)
        {
            var asArray = keyValuePair.Value as Array;

            if (asArray != null)
            {
                foreach (var item in asArray)
                {
                    yield return GetKeyValuePair(keyValuePair.Key, item);
                }
            }
            else
            {
                yield return GetKeyValuePair(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }

    private static KeyValuePair<string, string?> GetKeyValuePair(string key, object? value)
    {
        var asString = value as string;

        if (asString != null)
        {
            return new KeyValuePair<string, string?>(key, asString);
        }
        else
        {
            return new KeyValuePair<string, string?>(key, value?.ToString());
        }
    }
}
