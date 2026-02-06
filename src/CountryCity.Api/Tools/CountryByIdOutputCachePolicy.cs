using Microsoft.AspNetCore.OutputCaching;

namespace CountryCity.Api.Tools;

public sealed class CountryByIdOutputCachePolicy : AuthOutputCachePolicyBase
{
    public override ValueTask CacheRequestAsync(
        OutputCacheContext context, 
        CancellationToken cancellationToken)
    {
        context.Tags.Add("countries");

        var countryId = context.HttpContext.Request.RouteValues["countryId"]?.ToString();

        if (!string.IsNullOrWhiteSpace(countryId))
        {
            context.Tags.Add($"country:{countryId}");
        }

        return base.CacheRequestAsync(context, cancellationToken);
    }
}