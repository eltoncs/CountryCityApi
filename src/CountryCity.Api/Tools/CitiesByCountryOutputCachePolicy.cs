using Microsoft.AspNetCore.OutputCaching;

namespace CountryCity.Api.Tools;

public sealed class CitiesByCountryOutputCachePolicy : AuthOutputCachePolicyBase
{
    public override ValueTask CacheRequestAsync(
        OutputCacheContext context, 
        CancellationToken cancellationToken)
    {
        // Tag for broad eviction
        context.Tags.Add("cities");

        // Tag for per-country eviction
        var countryId = context.HttpContext.Request.RouteValues["countryId"]?.ToString();

        if (!string.IsNullOrWhiteSpace(countryId))
        {
            context.Tags.Add($"cities:{countryId}");
        }

        return base.CacheRequestAsync(context, cancellationToken);
    }
}