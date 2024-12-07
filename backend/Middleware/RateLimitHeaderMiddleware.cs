using Microsoft.AspNetCore.RateLimiting;

namespace JobHuntBackend.Middleware
{
  public class RateLimitHeaderMiddleware
  {
    private readonly RequestDelegate _next;

    public RateLimitHeaderMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var endpoint = context.GetEndpoint();
      var rateLimitMetadata = endpoint?.Metadata.GetMetadata<EnableRateLimitingAttribute>();

      if (rateLimitMetadata != null)
      {
        context.Response.OnStarting(() =>
        {
          context.Response.Headers["X-RateLimit-Policy"] = rateLimitMetadata.PolicyName;
          return Task.CompletedTask;
        });
      }

      await _next(context);
    }
  }

  public static class RateLimitHeaderMiddlewareExtensions
  {
    public static IApplicationBuilder UseRateLimitHeaders(
        this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<RateLimitHeaderMiddleware>();
    }
  }
}