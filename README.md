# AtleX.Web.Mvc

A small utility library with extensions to ASP.net MVC

## Installation

```
install-package AtleX.Web.Mvc -Pre
```

The package is hosted [on NuGet.org](https://www.nuget.org/packages/AtleX.Web.Mvc/)

## What does it do?

AtleX.Web.Mvc is a collection of extension methods, custom ActionResults and Output Cache
providers for ASP.net MVC.

### Controller extensions

* `HttpRedirectPermanent()`, does a HTTP/301 permanent redirect without outputting additional HTML
* `HttpRedirectTemporary()`, does a HTTP/302 temporary redirect without outputting additional HTML

### Output Cache providers

* `RedisOutputCacheProvider`, a drop-in replacement for the built-in ASP.net MVC `AspNetInternalProvider`

## Usage

### Controller extensions

Just add a `using`:

```csharp
using AtleX.Web.Mvc;
```

The methods can now be used in the action methods, for example:

```csharp
[HttpGet]
public ActionResult ToMyHomePage()
{
    return this.HttpRedirectTemporary("http://atlex.nl/");
}
```

### Redis Output Cache provider

Put the following in the `System.Web` section of the `Web.config` file:

```xml
<caching> 
    <outputCache defaultProvider="RedisOutputCacheProvider"> 
        <providers> 
            <clear />
            <add name="RedisOutputCacheProvider" 
                type="AtleX.Web.Mvc.OutputCache.Providers.RedisOutputCacheProvider, AtleX.Web.Mvc"
                connectionStringReference="redisServer" /> 
        </providers> 
    </outputCache>
</caching>
```

And add a connection string for your Redis server:

```xml
<connectionStrings>
    <add name="redisServer" connectionString="127.0.0.1,password=..."/>
</connectionStrings>
```

## Acknowledgements

AtleX.Web.Mvc uses the awesome [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis/) library.

## License

AtleX.Web.Mvc is released under the MIT license, as described in LICENSE.txt.