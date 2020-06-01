# jaytwo.UrlHelper

[![NuGet version (jaytwo.UrlHelper)](https://img.shields.io/nuget/v/jaytwo.UrlHelper.svg?style=flat-square)](https://www.nuget.org/packages/jaytwo.UrlHelper/)

Simple URL manipulation.  I wanted a way to manipulate a URL as a string without relying on transforming 
it to a `System.Uri` and back again.  This was not a performance or optimization reasons.  Rather, I sometimes
deal with imperfect systems and _less-than-properly-formatted_ URL's.  So, I wanted something to manipulate the
URL as a string instead of the `Uri` as an object.

At some point in my career I remember learning about a quirk in .NET that made `Uri.AbsoluteUri` and `Uri.ToString()` 
handle escaping/unescaping/reescaping differently.  I'm pretty sure it had to do with escaping slashes in the path).  
I'm also pretty sure this is corrected by now and doesn't matter... but what can I say... old habits die hard.  
It's easier in my brain to keep the strings as strings and avoid decoding/reencoding fromt the guts of `System.Uri` anyway.

## Installation

Add the NuGet package

```
PM> Install-Package jaytwo.UrlHelper
```

## Usage

### Query Operations:

```csharp
Url.SetQueryParameter("http://www.google.com/?foo=bar", "fizz", 0);
// returns "http://www.google.com/?foo=bar&fizz=0"

Url.GetQueryValue("http://www.google.com/?foo=bar", "foo");
// returns "bar"

Url.GetQueryValueAsArray("http://www.google.com/?foo=bar&foo=baz", "foo");
// returns { "bar", "baz" }

Url.RemoveQueryParameter("http://www.google.com/?foo=bar&fizz=buzz", "foo");
// returns "http://www.google.com/?fizz=buzz"

Url.GetQuery("http://www.google.com/?foo=bar");
// returns "?foo=bar"

Url.RemoveQuery("http://www.google.com/a/b?c=d");
// returns "http://www.google.com/a/b"
```

### Path Operations:

```csharp
Url.AppendPath("http://www.google.com/foo", "bar");
// returns "http://www.google.com/foo/bar"
Url.AppendPath("http://www.google.com/foo", "hello/{0}", new[] { "a b" });
// returns "http://www.google.com/foo/hello/a%20b"

Url.SetPath("http://www.google.com", "foo");
// returns "http://www.google.com/foo"
Url.SetPath("http://www.google.com/foo", "hello/{0}", new[] { "a b" });
// returns "http://www.google.com/hello/a%20b"
```

---

Made with &hearts; by Jake
