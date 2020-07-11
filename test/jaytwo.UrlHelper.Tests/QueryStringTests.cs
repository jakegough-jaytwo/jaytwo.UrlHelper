using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace jaytwo.UrlHelper.Tests
{
    public class QueryStringTests
    {
        public static IEnumerable<object[]> GetQueryString_object_data()
        {
            yield return new object[] { new { foo = "bar", fizz = "buzz" }, "foo=bar&fizz=buzz" };
            yield return new object[] { new { foo = new object[] { "bar", 123 }, fizz = "buzz" }, "foo=bar&foo=123&fizz=buzz" };
            yield return new object[] { new { foo = new[] { "bar", "baz" }, fizz = new[] { "buzz" } }, "foo=bar&foo=baz&fizz=buzz" };
            yield return new object[] { new { foo = "hello world" }, "foo=hello%20world" };
        }

        [Theory]
        [InlineData("foo=bar&fizz=buzz", "{ \"foo\": [ \"bar\" ], \"fizz\": [ \"buzz\" ] }")]
        [InlineData("foo=hello%20world", "{ \"foo\": [ \"hello world\" ] }")]
        public void ParseQueryString(string queryString, string expectedDictionaryJson)
        {
            // arrange

            // act
            var query = QueryString.Deserialize(queryString);

            // assert
            var expected = JsonConvert.DeserializeObject<IDictionary<string, string[]>>(expectedDictionaryJson);

            Assert.Equal(expected, query);
        }

        [Theory]
        [InlineData("{ \"foo\": [ \"bar\" ], \"fizz\": [ \"buzz\" ] }", "foo=bar&fizz=buzz")]
        [InlineData("{ \"foo\": [ \"hello world\" ] }", "foo=hello%20world")]
        public void GetQueryString_dictionary_string_stringarray(string dataJson, string expectedQueryString)
        {
            // arrange
            var data = JsonConvert.DeserializeObject<IDictionary<string, string[]>>(dataJson);

            // act
            var queryString = QueryString.Serialize(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }

        [Theory]
        [InlineData("{ \"foo\": [ 123 ], \"fizz\": [ \"buzz\" ] }", "foo=123&fizz=buzz")]
        [InlineData("{ \"foo\": [ \"hello world\" ] }", "foo=hello%20world")]
        public void GetQueryString_dictionary_string_objectarray(string dataJson, string expectedQueryString)
        {
            // arrange
            var data = JsonConvert.DeserializeObject<IDictionary<string, object[]>>(dataJson);

            // act
            var queryString = QueryString.Serialize(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }

        [Theory]
        [InlineData("{ \"foo\": \"bar\", \"fizz\": \"buzz\" }", "foo=bar&fizz=buzz")]
        [InlineData("{ \"foo\": \"hello world\" }", "foo=hello%20world")]
        public void GetQueryString_dictionary_string_string(string dataJson, string expectedQueryString)
        {
            // arrange
            var data = JsonConvert.DeserializeObject<IDictionary<string, string>>(dataJson);

            // act
            var queryString = QueryString.Serialize(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }

        [Theory]
        [InlineData("{ \"foo\": 123, \"fizz\": \"buzz\" }", "foo=123&fizz=buzz")]
        [InlineData("{ \"foo\": \"hello world\" }", "foo=hello%20world")]
        public void GetQueryString_dictionary_string_object(string dataJson, string expectedQueryString)
        {
            // arrange
            var data = JsonConvert.DeserializeObject<IDictionary<string, object>>(dataJson);

            // act
            var queryString = QueryString.Serialize(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }

        [Theory]
        [MemberData(nameof(GetQueryString_object_data))]
        public void GetQueryString_object(object data, string expectedQueryString)
        {
            // arrange

            // act
            var queryString = QueryString.Serialize(data);

            // assert
            Assert.Equal(expectedQueryString, queryString);
        }
    }
}
