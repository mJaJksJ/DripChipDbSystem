using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.ResponseTypes;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DripChipDbSystem.Api.Controllers.AuthController
{
    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        [HttpPost("/registration")]
        [ProducesResponseType(typeof(AccountResponseContract), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> RegistrationAsync([FromBody] AccountRequestContract contract)
        {
            if (Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                throw new Forbidden403Exception();
            }

            var response = await _authService.AddAccountAsync(contract);
            return new Created201Result(response);
        }
    }
}
