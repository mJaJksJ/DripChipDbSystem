using Microsoft.AspNetCore.Builder;

namespace DripChipDbSystem.Middlewares.BasicLogOutMiddleware
{
    /// <summary>
    /// Расширения для <see cref="HttpResponseMiddleware"/>
    /// </summary>
    public static class BasicLogOutMiddlewareExtensions
    {
        /// <summary>
        /// Использовать <see cref="BasicLogOutMiddleware"/>
        /// </summary>
        public static IApplicationBuilder UseBasicLogOutMiddlewareExtensions(this IApplicationBuilder app)
        {
            return app.UseMiddleware<BasicLogOutMiddleware>();
        }
    }
}
