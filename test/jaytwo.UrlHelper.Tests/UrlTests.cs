using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace jaytwo.UrlHelper.Tests
{
    public class UrlTests
    {
        [Theory]
        [InlineData("http://www.google.com", "http://www.google.com")]
        [InlineData("http://www.google.com/a/b", "http://www.google.com/a/b")]
        [InlineData("http://www.google.com/a/b?c=d", "http://www.google.com/a/b")]
        [InlineData("http://www.google.com/a/b/?c=d", "http://www.google.com/a/b/")]
        [InlineData("/a/b/?c=d", "/a/b/")]
        [InlineData("../a/b/?c=d", "../a/b/")]
        public void RemoveQuery(string baseUrl, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.RemoveQuery(baseUrl);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "foo", "http://www.google.com/")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "foo", "http://www.google.com/?fizz=buzz")]
        [InlineData("http://www.google.com/?foo=bar&fizz=buzz", "fizz", "http://www.google.com/?foo=bar")]
        [InlineData("/?foo=bar&fizz=buzz", "fizz", "/?foo=bar")]
        [InlineData("../?foo=bar&fizz=buzz", "fizz", "../?foo=bar")]
        public void RemoveQueryParameter(string baseUrl, string queryParameter, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.RemoveQueryParameter(baseUrl, queryParameter);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", "foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/", "/foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/foo", "bar", "http://www.google.com/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "bar", "http://www.google.com/bar?hello=world")]
        [InlineData("http://www.google.com/foo", "/bar", "http://www.google.com/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "/bar", "http://www.google.com/bar?hello=world")]
        [InlineData("/foo", "/bar", "/bar")]
        [InlineData("foo", "/bar", "/bar")]
        [InlineData("/foo", "bar", "bar")]
        [InlineData("foo", "bar", "bar")]
        [InlineData("foo", "foo%2Fbar", "foo%2Fbar")]
        [InlineData("/foo?hello=world", "/bar", "/bar?hello=world")]
        [InlineData("/foo?hello=world", "bar", "bar?hello=world")]
        [InlineData("?hello=world", "bar", "bar?hello=world")]
        [InlineData("", "bar", "bar")]
        public void SetPath(string baseUrl, string path, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.SetPath(baseUrl, path);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 0, "xyz", "http://www.google.com/xyz/def/ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 0, "hello world", "http://www.google.com/hello%20world/def/ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 0, "", "http://www.google.com//def/ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 0, null, "http://www.google.com//def/ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 1, "xyz", "http://www.google.com/abc/xyz/ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 1, "hello world", "http://www.google.com/abc/hello%20world/ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 1, "", "http://www.google.com/abc//ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 1, null, "http://www.google.com/abc//ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 2, "xyz", "http://www.google.com/abc/def/xyz/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 2, "hello world", "http://www.google.com/abc/def/hello%20world/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 2, "", "http://www.google.com/abc/def//?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 2, null, "http://www.google.com/abc/def//?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 3, "xyz", "http://www.google.com/abc/def/ghi/xyz?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 3, "hello world", "http://www.google.com/abc/def/ghi/hello%20world?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 3, "", "http://www.google.com/abc/def/ghi/?foo=bar/biz")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 3, null, "http://www.google.com/abc/def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 0, "xyz", "/xyz/def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 0, "hello world", "/hello%20world/def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 0, "", "//def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 0, null, "//def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 1, "xyz", "/abc/xyz/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 1, "hello world", "/abc/hello%20world/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 1, "", "/abc//ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 1, null, "/abc//ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 2, "xyz", "/abc/def/xyz/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 2, "hello world", "/abc/def/hello%20world/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 2, "", "/abc/def//?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 2, null, "/abc/def//?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 3, "xyz", "/abc/def/ghi/xyz?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 3, "hello world", "/abc/def/ghi/hello%20world?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 3, "", "/abc/def/ghi/?foo=bar/biz")]
        [InlineData("/abc/def/ghi/?foo=bar/biz", 3, null, "/abc/def/ghi/?foo=bar/biz")]
        [InlineData("abc/def/ghi/?foo=bar/biz", 0, "xyz", "xyz/def/ghi/?foo=bar/biz")]
        [InlineData("abc/def/ghi/?foo=bar/biz", 1, "xyz", "abc/xyz/ghi/?foo=bar/biz")]
        [InlineData("abc/def/ghi/?foo=bar/biz", 2, "xyz", "abc/def/xyz/?foo=bar/biz")]
        [InlineData("abc/def/ghi/?foo=bar/biz", 3, "xyz", "abc/def/ghi/xyz?foo=bar/biz")]
        [InlineData("../abc/def/ghi/?foo=bar/biz", 0, "xyz", "xyz/abc/def/ghi/?foo=bar/biz")]
        [InlineData("../abc/def/ghi/?foo=bar/biz", 1, "xyz", "../xyz/def/ghi/?foo=bar/biz")]
        [InlineData("../abc/def/ghi/?foo=bar/biz", 2, "xyz", "../abc/xyz/ghi/?foo=bar/biz")]
        [InlineData("../abc/def/ghi/?foo=bar/biz", 3, "xyz", "../abc/def/xyz/?foo=bar/biz")]
        [InlineData("../abc/def/ghi/?foo=bar/biz", 4, "xyz", "../abc/def/ghi/xyz?foo=bar/biz")]
        public void SetPathSegment(string url, int index, string value, string expected)
        {
            // arrange

            // act
            var actual = Url.SetPathSegment(url, index, value);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("http://www.google.com", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        [InlineData("http://www.google.com/foo", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        [InlineData("/foo", "hello/{0}", new[] { "a b" }, "hello/a%20b")]
        [InlineData("foo", "hello/{0}", new[] { "a b" }, "hello/a%20b")]
        [InlineData("/foo?x=y", "hello/{0}", new[] { "a b" }, "hello/a%20b?x=y")]
        [InlineData("foo?x=y", "hello/{0}", new[] { "a b" }, "hello/a%20b?x=y")]
        public void SetPath_Format(string baseUrl, string pathFormat, string[] formatArgs, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.SetPath(baseUrl, pathFormat, formatArgs);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", "foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/", "/foo", "http://www.google.com/foo")]
        [InlineData("http://www.google.com/foo", "bar", "http://www.google.com/foo/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "bar", "http://www.google.com/foo/bar?hello=world")]
        [InlineData("http://www.google.com/foo", "/bar", "http://www.google.com/foo/bar")]
        [InlineData("http://www.google.com/foo?hello=world", "/bar", "http://www.google.com/foo/bar?hello=world")]
        [InlineData("", "foo/bar", "foo/bar")]
        [InlineData("/foo", "/bar", "/foo/bar")]
        [InlineData("/foo", "bar", "/foo/bar")]
        [InlineData("foo", "bar", "foo/bar")]
        [InlineData("/foo?hello=world", "/bar", "/foo/bar?hello=world")]
        [InlineData("/foo?hello=world", "bar", "/foo/bar?hello=world")]
        public void AppendPath(string baseUrl, string path, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.AppendPath(baseUrl, path);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com/{0}/foo.bar", new[] { "banana" }, "http://www.google.com/banana/foo.bar")]
        [InlineData("http://www.google.com/{0}/foo.bar", new[] { default(string) }, "http://www.google.com//foo.bar")]
        [InlineData("{0}/foo.bar", new string[] { "abc" }, "abc/foo.bar")]
        public void Format(string format, string[] args, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.Format(format, args);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com/{0}/foo.bar", new object[] { 1 }, "http://www.google.com/1/foo.bar")]
        [InlineData("http://www.google.com/{0}/foo.bar", new object[] { null }, "http://www.google.com//foo.bar")]
        [InlineData("{0}/foo.bar", new object[] { "abc" }, "abc/foo.bar")]
        public void Format_value_object(string format, object[] args, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.Format(format, args);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("", "banana", "banana")]
        [InlineData("", "/banana", "/banana")]
        [InlineData("/", "/banana", "/banana")]
        [InlineData("banana", "/rama", "banana/rama")]
        [InlineData("banana", "rama", "banana/rama")]
        [InlineData("banana/", "rama", "banana/rama")]
        [InlineData("banana/", "/rama", "banana/rama")]
        [InlineData("/banana", "/rama", "/banana/rama")]
        [InlineData("/banana", "rama", "/banana/rama")]
        [InlineData("/banana/", "rama", "/banana/rama")]
        [InlineData("/banana/", "/rama", "/banana/rama")]
        [InlineData("http://www.google.com", "banana", "http://www.google.com/banana")]
        [InlineData("http://www.google.com/", "banana", "http://www.google.com/banana")]
        [InlineData("http://www.google.com/foo", "banana", "http://www.google.com/foo/banana")]
        [InlineData("http://www.google.com/foo/", "banana", "http://www.google.com/foo/banana")]
        [InlineData("http://www.google.com/foo/", "banana/", "http://www.google.com/foo/banana/")]
        [InlineData("http://www.google.com/foo/", "banana/rama", "http://www.google.com/foo/banana/rama")]
        [InlineData("http://www.google.com/foo/", "banana/rama?fizz=buzz", "http://www.google.com/foo/banana/rama?fizz=buzz")]
        public void Combine(string format, string segment, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.Combine(format, segment);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", new string[] { "banana" }, "http://www.google.com/banana")]
        [InlineData("http://www.google.com/", new string[] { "banana" }, "http://www.google.com/banana")]
        [InlineData("http://www.google.com/foo", new string[] { "banana" }, "http://www.google.com/foo/banana")]
        [InlineData("http://www.google.com/foo/", new string[] { "banana" }, "http://www.google.com/foo/banana")]
        [InlineData("http://www.google.com/foo/", new string[] { "banana/" }, "http://www.google.com/foo/banana/")]
        [InlineData("http://www.google.com/foo/", new string[] { "banana/rama" }, "http://www.google.com/foo/banana/rama")]
        [InlineData("http://www.google.com/foo/", new string[] { "banana", "rama" }, "http://www.google.com/foo/banana/rama")]
        [InlineData("http://www.google.com/foo/", new string[] { "banana", "rama?fizz=buzz" }, "http://www.google.com/foo/banana/rama?fizz=buzz")]
        public void Combine_multiple_segments(string format, string[] segments, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.Combine(format, segments);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com", "hello/{0}", new[] { "a b" }, "http://www.google.com/hello/a%20b")]
        [InlineData("http://www.google.com/foo", "hello/{0}", new[] { "a b" }, "http://www.google.com/foo/hello/a%20b")]
        [InlineData("/foo", "hello/{0}", new[] { "a b" }, "/foo/hello/a%20b")]
        [InlineData("/foo?x=y", "hello/{0}", new[] { "a b" }, "/foo/hello/a%20b?x=y")]
        public void AppendPath_Format(string baseUrl, string pathFormat, string[] formatArgs, string expectedUrl)
        {
            // arrange

            // act
            var url = Url.AppendPath(baseUrl, pathFormat, formatArgs);

            // assert
            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", "buzz", "http://www.google.com/?foo=bar&fizz=buzz")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("/a?foo=bar", "fizz", "buzz", "/a?foo=bar&fizz=buzz")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", "/a?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", "/a?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", null, "/a?fi%26zz=bu%20zz&foo=")]
        [InlineData("?fizz=buzz", "foo", "bar", "?fizz=buzz&foo=bar")]
        [InlineData("", "foo", "bar", "?foo=bar")]
        public void SetQueryParameter(string baseUrl, string key, string value, string expectedUrl)
        {
            // arrange

            // act
            var uri = Url.SetQueryParameter(baseUrl, key, value);

            // assert
            Assert.Equal(expectedUrl, uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar", "fizz", 0, "http://www.google.com/?foo=bar&fizz=0")]
        [InlineData("http://www.google.com/?foo=bar", "fi&zz", "bu zz", "http://www.google.com/?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("http://www.google.com/?fi%26zz=bu%20zz", "foo", "bar", "http://www.google.com/?fi%26zz=bu%20zz&foo=bar")]
        [InlineData("/a?foo=bar", "fizz", "buzz", "/a?foo=bar&fizz=buzz")]
        [InlineData("/a?foo=bar", "fi&zz", "bu zz", "/a?foo=bar&fi%26zz=bu%20zz")]
        [InlineData("/a?fi%26zz=bu%20zz", "foo", "bar", "/a?fi%26zz=bu%20zz&foo=bar")]
        public void SetQueryParameter_value_object(string baseUrl, string key, object value, string expectedUrl)
        {
            // arrange

            // act
            var uri = Url.SetQueryParameter(baseUrl, key, value);

            // assert
            Assert.Equal(expectedUrl, uri);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar&fizz=0&fizz=1&bar=1", "?foo=bar&fizz=0&fizz=1&bar=1")]
        [InlineData("http://www.google.com/?foo=bar&fizz=0", "?foo=bar&fizz=0")]
        [InlineData("http://www.google.com/?foo=bar&", "?foo=bar&")]
        [InlineData("http://www.google.com/?foo=bar", "?foo=bar")]
        [InlineData("http://www.google.com/?", "?")]
        [InlineData("http://www.google.com/", "")]
        public void GetQuery(string url, string expected)
        {
            // arrange

            // act
            var query = Url.GetQuery(url);

            // assert
            Assert.Equal(expected, query);
        }

        [Theory]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", "/abc/def/ghi/")]
        [InlineData("http://www.google.com/abc/def/ghi", "/abc/def/ghi")]
        [InlineData("http://www.google.com/abc//def/ghi", "/abc//def/ghi")]
        [InlineData("http://www.google.com/", "/")]
        [InlineData("http://www.google.com", "")]
        public void GetPath(string url, string expected)
        {
            // arrange

            // act
            var query = Url.GetPath(url);

            // assert
            Assert.Equal(expected, query);
        }

        [Theory]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", "abc|def|ghi|")]
        [InlineData("http://www.google.com/abc/def/ghi", "abc|def|ghi")]
        [InlineData("http://www.google.com/abc//def/ghi", "abc||def|ghi")]
        public void GetPathSegments(string url, string expected)
        {
            // arrange
            var expectedPathSegments = expected.Split('|');

            // act
            var actual = Url.GetPathSegments(url);

            // assert
            Assert.Equal(expectedPathSegments, actual);
        }

        [Theory]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 0, "abc")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 1, "def")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 2, "ghi")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 3, "")]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 4, null)]
        [InlineData("http://www.google.com/abc/def/ghi/?foo=bar/biz", 99, null)]
        public void GetPathSegment(string url, int index, string expected)
        {
            // arrange

            // act
            var actual = Url.GetPathSegment(url, index);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("http://www.google.com/")]
        [InlineData("http://www.google.com")]
        [InlineData("http://www.google.com/?foo")]
        [InlineData("http://www.google.com?foo")]
        public void GetPathSegments_returns_empty_array(string url)
        {
            // arrange

            // act
            var actual = Url.GetPathSegments(url);

            // assert
            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar&fizz=0&fizz=1&bar=1", "foo", "bar")]
        [InlineData("http://www.google.com/?foo=bar&fizz=0", "fizz", "0")]
        [InlineData("http://www.google.com/?foo=bar&", "foo", "bar")]
        [InlineData("http://www.google.com/?foo=bar", "foo", "bar")]
        public void GetQueryValue(string url, string key, string expected)
        {
            // arrange

            // act
            var query = Url.GetQueryValue(url, key);

            // assert
            Assert.Equal(expected, query);
        }

        [Theory]
        [InlineData("http://www.google.com/?foo=bar&fizz=0&fizz=1&foo=1", "foo", new[] { "bar", "1" })]
        [InlineData("http://www.google.com/?foo=bar&fizz=0", "fizz", new[] { "0" })]
        [InlineData("http://www.google.com/?foo=bar&", "foo", new[] { "bar" })]
        [InlineData("http://www.google.com/?foo=bar", "foo", new[] { "bar" })]
        public void GetQueryValueAsArray(string url, string key, string[] expected)
        {
            // arrange

            // act
            var query = Url.GetQueryValueAsArray(url, key);

            // assert
            Assert.Equal(expected, query);
        }

        [Theory]
        [InlineData("", "/hello", "foo=bar", "/hello?foo=bar")]
        [InlineData("http://www.google.com", "/hello", "foo=bar", "http://www.google.com/hello?foo=bar")]
        public void SetQuery_before_SetPath(string url, string path, string query, string expectedUrl)
        {
            // arrange
            url = Url.SetQuery(url, query);

            // act
            url = Url.SetPath(url, path);

            // assert
            Assert.Equal(expectedUrl, url);
        }
    }
}
