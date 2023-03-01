using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DripChipDbSystem.Authentification;
using Microsoft.AspNetCore.Http;

namespace DripChipDbSystem.Middlewares.BasicLogOutMiddleware
{
    /// <summary>
    /// Миддлвар очистки авторизированного пользователя
    /// </summary>
    public class BasicLogOutMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// .ctor
        /// </summary>
        public BasicLogOutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Выполнить миддлвар
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity is ClaimsIdentity identity)
            {
                var claim = context.User.Claims.SingleOrDefault(x => x.Value == BasicAuth.Sid);
                if (claim != null)
                {
                    identity.RemoveClaim(claim);
                }
            }

            await _next(context);
        }
    }
}
