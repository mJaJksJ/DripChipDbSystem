using System;
using System.Threading.Tasks;
using DripChipDbSystem.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DripChipDbSystem.Middlewares.HttpResponseMiddleware
{
    /// <summary>
    /// Миддлвар обработки ответов на запрос
    /// </summary>
    public class HttpResponseMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// .ctor
        /// </summary>
        public HttpResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Выполнить миддлвар
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequest400Exception ex400)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(ex400.Message);
            }
            catch (NotFound404Exception ex404)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(ex404.Message);
            }
            catch (Conflict409Exception ex409)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(ex409.Message);
            }
            catch (Unauthorized401Exception ex401)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(ex401.Message);
            }
            catch (Forbidden403Exception ex403)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(ex403.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }
    }
}
