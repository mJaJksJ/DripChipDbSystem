using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Http;
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
        private readonly AccountService _accountService;

        public AuthController(
            AuthService authService,
            AccountService accountService)
        {
            _authService = authService;
            _accountService = accountService;
        }

        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        [HttpPost("/registration")]
        [ProducesResponseType(typeof(AccountResponseContract), 201)]
        [ProducesResponseType(typeof(AccountResponseContract), 400)]
        [ProducesResponseType(typeof(AccountResponseContract), 403)]
        [ProducesResponseType(typeof(AccountResponseContract), 409)]
        public async Task<IActionResult> RegistrationAsync([FromBody] AccountRequestContract contract)
        {
            if (Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                throw new Forbidden403Exception();
            }

            await _accountService.EnsureAccountNotExists(contract);
            var response = await _authService.AddAccountAsync(contract);
            return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
