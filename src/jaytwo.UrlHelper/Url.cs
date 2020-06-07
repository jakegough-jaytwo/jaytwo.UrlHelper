using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.UrlHelper
{
    public static class Url
    {
        public static string Format(string format, params string[] formatArgs)
        {
            var escapedArgs = formatArgs?.Select(x => Uri.EscapeDataString(x ?? string.Empty)).ToArray();
            var path = string.Format(format, escapedArgs);

            VerifyValidUrl(path);

            return path;
        }

        public static string Format(string format, params object[] formatArgs)
        {
            var argsAsStrings = formatArgs.Select(Convert.ToString).ToArray();
            return Format(format, argsAsStrings);
        }

        public static string Combine(string baseUrl, params string[] segments)
        {
            var result = baseUrl ?? string.Empty;

            foreach (var segment in segments)
            {
                result = Combine(result, segment, throwOnInvalid: false);
            }

            VerifyValidUrl(result);

            return result;
        }

        public static string Combine(string baseUrl, string segment) => Combine(baseUrl, segment, throwOnInvalid: true);

        public static string GetPath(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            string result;
            var withoutQuery = RemoveQuery(url);

            if (Uri.IsWellFormedUriString(withoutQuery, UriKind.Absolute))
            {
                var indexOfDoubleSlash = withoutQuery.IndexOf("//");
                var indexOfSlashAfterHost = withoutQuery.IndexOf("/", indexOfDoubleSlash + 2);

                if (indexOfSlashAfterHost >= 0)
                {
                    result = withoutQuery.Substring(indexOfSlashAfterHost);
                }
                else
                {
                    result = string.Empty;
                }
            }
            else
            {
                result = withoutQuery;
            }

            if (!string.IsNullOrEmpty(result))
            {
                VerifyValidUrl(result);
            }

            return result;
        }

        public static string SetPath(string url, string path)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var query = GetQuery(url);
            var withoutQuery = RemoveQuery(url);

            if (Uri.IsWellFormedUriString(withoutQuery, UriKind.Absolute))
            {
                var indexOfDoubleSlash = withoutQuery.IndexOf("//");
                var indexOfSlashAfterHost = withoutQuery.IndexOf("/", indexOfDoubleSlash + 2);

                var prefix = (indexOfSlashAfterHost >= 0)
                    ? withoutQuery.Substring(0, indexOfSlashAfterHost)
                    : withoutQuery;

                return SetQuery(prefix + "/" + path.TrimStart('/'), query);
            }
            else
            {
                var prefix = withoutQuery.StartsWith("/") || path.StartsWith("/")
                    ? "/"
                    : string.Empty;

                return SetQuery(prefix + path.TrimStart('/'), query);
            }
        }

        public static string SetPath(string url, string pathFormat, params string[] formatArgs)
        {
            var path = Format(pathFormat, formatArgs);
            return SetPath(url, path);
        }

        public static string SetPath(string url, string pathFormat, params object[] formatArgs)
        {
            var path = Format(pathFormat, formatArgs);
            return SetPath(url, path);
        }

        public static string AppendPath(string url, string path)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var oldPath = GetPath(url);
            var newPath = Combine(oldPath, path);
            return SetPath(url, newPath);
        }

        public static string AppendPath(string url, string pathFormat, params string[] formatArgs)
        {
            var path = Format(pathFormat, formatArgs);
            return AppendPath(url, path);
        }

        public static string AppendPath(string url, string pathFormat, params object[] formatArgs)
        {
            var path = Format(pathFormat, formatArgs);
            return AppendPath(url, path);
        }

        public static string GetQuery(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var queryStartPosition = url.IndexOf('?');

            if (queryStartPosition >= 0)
            {
                return url.Substring(queryStartPosition);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string[] GetQueryValueAsArray(string url, string key)
        {
            var query = GetQuery(url);
            var parsedQuery = QueryString.Deserialize(query);

            if (parsedQuery.ContainsKey(key))
            {
                return parsedQuery[key];
            }

            return new string[] { };
        }

        public static string GetQueryValue(string url, string key) => GetQueryValueAsArray(url, key).FirstOrDefault();

        public static string RemoveQuery(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var queryStartPosition = url.IndexOf('?');

            var result = (queryStartPosition < 0)
                ? url
                : url.Substring(0, queryStartPosition);

            return result;
        }

        public static string SetQuery(string url, string query)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            string result;
            var withoutQuery = RemoveQuery(url);

            if (!string.IsNullOrEmpty(query))
            {
                result = withoutQuery + "?" + query.TrimStart('?');
            }
            else
            {
                result = withoutQuery;
            }

            VerifyValidUrl(result);
            return result;
        }

        public static string SetQuery(string url, object data)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var query = QueryString.Serialize(data);
            return SetQuery(url, query);
        }

        public static string SetQueryParameter(string url, string key, string value)
        {
            var parsedQuery = QueryString.Deserialize(GetQuery(url));
            parsedQuery[key] = new[] { value };

            var query = QueryString.Serialize(parsedQuery);
            return SetQuery(url, query);
        }

        public static string SetQueryParameter(string url, string key, object value) => SetQueryParameter(url, key, $"{value}");

        public static string AddQueryParameter(string url, string key, string value)
        {
            var parsedQuery = QueryString.Deserialize(GetQuery(url));
            if (parsedQuery.ContainsKey(key))
            {
                var newValues = new List<string>(parsedQuery[key]);
                newValues.Add(value);
                parsedQuery[key] = newValues.ToArray();
            }
            else
            {
                parsedQuery[key] = new[] { value };
            }

            var query = QueryString.Serialize(parsedQuery);
            return SetQuery(url, query);
        }

        public static string AddQueryParameter(string url, string key, object value) => AddQueryParameter(url, key, $"{value}");

        public static string RemoveQueryParameter(string url, string key)
        {
            var parsedQuery = QueryString.Deserialize(GetQuery(url));

            if (parsedQuery.ContainsKey(key))
            {
                parsedQuery.Remove(key);

                var query = QueryString.Serialize(parsedQuery);
                return SetQuery(url, query);
            }
            else
            {
                return url;
            }
        }

        private static string Combine(string baseUrl, string segment, bool throwOnInvalid)
        {
            string result;

            if (string.IsNullOrEmpty(baseUrl))
            {
                result = segment;
            }
            else
            {
                result = baseUrl;

                if (!result.EndsWith("/"))
                {
                    result += "/";
                }

                result += (segment ?? string.Empty).TrimStart('/');
            }

            if (throwOnInvalid)
            {
                VerifyValidUrl(result);
            }

            return result;
        }

        private static void VerifyValidUrl(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                throw new InvalidOperationException($"Resulting URL is not a well-formed relative or absolute URI string: '{url}'.");
            }
        }
    }
}
