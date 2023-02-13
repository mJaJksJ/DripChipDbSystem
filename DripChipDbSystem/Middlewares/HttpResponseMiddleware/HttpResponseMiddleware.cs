using System.Threading.Tasks;
using DripChipDbSystem.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DripChipDbSystem.Middlewares.HttpResponseMiddleware
{
    public class HttpResponseMiddleware
    {
        public const string ResultKey = "result";

        private readonly RequestDelegate _next;
        public HttpResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequest400Exception ex400)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(ex400.Data[ResultKey]);
            }
            catch (NotFound404Exception ex404)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(ex404.Data[ResultKey]);
            }
        }
    }
}
