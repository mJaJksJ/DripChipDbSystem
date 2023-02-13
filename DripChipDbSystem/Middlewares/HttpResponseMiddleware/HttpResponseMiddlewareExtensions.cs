using Microsoft.AspNetCore.Builder;

namespace DripChipDbSystem.Middlewares.HttpResponseMiddleware
{
    public static class HttpResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpResponseMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpResponseMiddleware>();
        }
    }
}
