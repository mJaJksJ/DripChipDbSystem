using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using DripChipDbSystem.Authentification;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DripChipDbSystem.Utils
{
    /// <summary>
    /// Инструменты работы с авторизацией
    /// </summary>
    public static class AuthUtils
    {
        /// <summary>
        /// Получить Id авторизированного пользователя
        /// </summary>
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var id = user.Claims.SingleOrDefault(_ => _.Type == BasicAuth.Sid)?.Value;
            return id is null ? null : int.Parse(id);
        }
    }
}
