using CountryCity.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CountryCity.Api.Middleware
{
    public class ExceptionHandling : IMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException de)
            {
                // If response already started, we can't change headers/status safely.
                if (context.Response.HasStarted) throw;

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Domain validation error",
                    Detail = de.Message,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
            }
            catch (Exception)
            {
                if (context.Response.HasStarted) throw;

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new { message = "Unexpected error." });
            }
        }
    }
}
