using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if ( /*уже авторизирован*/false)
            {
                return new ObjectResult(new AccountResponseContract()) { StatusCode = StatusCodes.Status403Forbidden };
            }

            await _accountService.EnsureAccountNotExists(contract);
            var response = await _authService.AddAccountAsync(contract);
            return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
