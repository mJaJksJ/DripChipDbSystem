using Microsoft.AspNetCore.Builder;

namespace DripChipDbSystem.Middlewares.HttpResponseMiddleware
{
    /// <summary>
    /// Расширения для <see cref="HttpResponseMiddleware"/>
    /// </summary>
    public static class HttpResponseMiddlewareExtensions
    {
        /// <summary>
        /// Использовать <see cref="HttpResponseMiddleware"/>
        /// </summary>
        public static IApplicationBuilder UseHttpResponseMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpResponseMiddleware>();
        }
    }
}
