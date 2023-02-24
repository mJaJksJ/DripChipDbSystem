using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Authentification;
using Microsoft.AspNetCore.Authorization;

namespace DripChipDbSystem.Api.Controllers.AccountController
{
    /// <summary>
    /// Аккаунт пользователя
    /// </summary>
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Получение информации об аккаунте пользователя
        /// </summary>
        [HttpGet("/accounts/{accountId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AccountResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAccountAsync([IdValidation] int accountId)
        {
            var response = await _accountService.GetAccountAsync(accountId);
            return Ok(response);
        }

        /// <summary>
        /// Поиск аккаунтов пользователей по параметрам
        /// </summary>
        [HttpGet("/accounts/search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AccountResponseContract>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        public async Task<IActionResult> SearchAsync(
            [FromQuery] string firstName,
            [FromQuery] string lastName,
            [FromQuery] string email,
            [FromQuery][FromValidation] int? from,
            [FromQuery][SizeValidation] int? size)
        {
            var response = await _accountService.SearchAsync(
                firstName,
                lastName,
                email,
                from,
                size);
            return Ok(response);
        }

        /// <summary>
        /// Обновление данных аккаунта пользователя
        /// </summary>
        [HttpPut("/accounts/{accountId:int}")]
        [ProducesResponseType(typeof(AccountResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> UpdateAccountAsync(
            [IdValidation] int accountId,
            AccountRequestContract contract)
        {
            var response = await _accountService.UpdateAccountAsync(accountId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление аккаунта пользователя
        /// </summary>
        [HttpDelete("/accounts/{accountId:int}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<IActionResult> DeleteAccountAsync([IdValidation] int accountId)
        {
            await _accountService.DeleteAccountAsync(accountId);
            return Ok();
        }
    }
}
