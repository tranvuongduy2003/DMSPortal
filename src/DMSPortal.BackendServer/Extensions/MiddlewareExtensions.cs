using DMSPortal.BackendServer.Helpers;

namespace DMSPortal.BackendServer.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorWrapping(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorWrappingMiddleware>();
    }
}