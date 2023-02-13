using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DripChipDbSystem.Api.Controllers.AccountController
{
    /// <summary>
    /// Аккаунт пользователя
    /// </summary>
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Получение информации об аккаунте пользователя
        /// </summary>
        [HttpGet("/accounts/{accountId} ")]
        [ProducesResponseType(typeof(AccountResponseContract), 200)]
        [ProducesResponseType(typeof(AccountResponseContract), 400)]
        [ProducesResponseType(typeof(AccountResponseContract), 401)]
        [ProducesResponseType(typeof(AccountResponseContract), 404)]
        public async Task<IActionResult> RegistrationAsync([AccountId(typeof(AccountResponseContract))] int accountId)
        {
            var response = await _accountService.GetAccountAsync(accountId);
            return Ok(response);
        }
    }
}
