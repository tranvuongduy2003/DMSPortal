using DMSPortal.BackendServer.Helpers;

namespace DMSPortal.BackendServer.Extentions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorWrapping(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorWrappingMiddleware>();
    }
}