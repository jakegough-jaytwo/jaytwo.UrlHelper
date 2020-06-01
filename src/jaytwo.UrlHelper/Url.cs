using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.UrlHelper
{
    public static class Url
    {
        public static string GetPath(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var withoutQuery = RemoveQuery(url);

            if (Uri.IsWellFormedUriString(withoutQuery, UriKind.Absolute))
            {
                var indexOfDoubleSlash = withoutQuery.IndexOf("//");
                var indexOfSlashAfterHost = withoutQuery.IndexOf("/", indexOfDoubleSlash + 2);

                if (indexOfSlashAfterHost >= 0)
                {
                    return withoutQuery.Substring(indexOfSlashAfterHost);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return withoutQuery;
            }
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
            var escapedArgs = formatArgs?.Select(Uri.EscapeDataString).ToArray();
            var path = string.Format(pathFormat, escapedArgs);
            return SetPath(url, path);
        }

        public static string SetPath(string url, string pathFormat, params object[] formatArgs)
        {
            var escapedArgs = formatArgs?.Select(x => Uri.EscapeDataString($"{x}")).ToArray();
            return SetPath(url, pathFormat, escapedArgs);
        }

        public static string AppendPath(string url, string path)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var oldPath = GetPath(url);

            var newPath = oldPath;
            if (!newPath.EndsWith("/"))
            {
                newPath += "/";
            }

            newPath += path.TrimStart('/');

            return SetPath(url, newPath);
        }

        public static string AppendPath(string url, string pathFormat, params string[] formatArgs)
        {
            var escapedArgs = formatArgs?.Select(Uri.EscapeDataString).ToArray();
            var path = string.Format(pathFormat, escapedArgs);
            return AppendPath(url, path);
        }

        public static string AppendPath(string url, string pathFormat, params object[] formatArgs)
        {
            var escapedArgs = formatArgs?.Select(x => Uri.EscapeDataString($"{x}")).ToArray();
            return AppendPath(url, pathFormat, escapedArgs);
        }

        public static string GetQuery(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var queryStartPosition = url.IndexOf('?');

            if (queryStartPosition > 0)
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
            var parsedQuery = QueryStringUtility.ParseQueryString(query);

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

            var queryStartPosition = url.LastIndexOf('?');

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

            var withoutQuery = RemoveQuery(url);

            if (!string.IsNullOrEmpty(query))
            {
                return withoutQuery + "?" + query.TrimStart('?');
            }
            else
            {
                return withoutQuery;
            }
        }

        public static string SetQuery(string url, object data)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var query = QueryStringUtility.GetQueryString(data);
            return SetQuery(url, query);
        }

        public static string SetQueryParameter(string url, string key, string value)
        {
            var parsedQuery = QueryStringUtility.ParseQueryString(GetQuery(url));
            parsedQuery[key] = new[] { value };

            var query = QueryStringUtility.GetQueryString(parsedQuery);
            return SetQuery(url, query);
        }

        public static string SetQueryParameter(string url, string key, object value) => SetQueryParameter(url, key, $"{value}");

        public static string AddQueryParameter(string url, string key, string value)
        {
            var parsedQuery = QueryStringUtility.ParseQueryString(GetQuery(url));
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

            var query = QueryStringUtility.GetQueryString(parsedQuery);
            return SetQuery(url, query);
        }

        public static string AddQueryParameter(string url, string key, object value) => AddQueryParameter(url, key, $"{value}");

        public static string RemoveQueryParameter(string url, string key)
        {
            var parsedQuery = QueryStringUtility.ParseQueryString(GetQuery(url));

            if (parsedQuery.ContainsKey(key))
            {
                parsedQuery.Remove(key);

                var query = QueryStringUtility.GetQueryString(parsedQuery);
                return SetQuery(url, query);
            }
            else
            {
                return url;
            }
        }
    }
}
