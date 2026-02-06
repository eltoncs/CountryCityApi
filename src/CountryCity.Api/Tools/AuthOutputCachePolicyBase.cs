using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace CountryCity.Api.Tools;

public class AuthOutputCachePolicyBase : IOutputCachePolicy
{
    public virtual ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var request = context.HttpContext.Request;

        var cacheableMethod = HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method);

        context.EnableOutputCaching = true;
        context.AllowCacheLookup = cacheableMethod;
        context.AllowCacheStorage = cacheableMethod;
        context.AllowLocking = true;

        // Common vary rules for your app
        context.CacheVaryByRules.RouteValueNames = new[] { "countryId" };
        context.CacheVaryByRules.HeaderNames = new[] { "Authorization" };

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    public virtual ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var response = context.HttpContext.Response;

        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
            context.AllowCacheStorage = false;

        if (response.StatusCode != StatusCodes.Status200OK)
            context.AllowCacheStorage = false;

        return ValueTask.CompletedTask;
    }
}