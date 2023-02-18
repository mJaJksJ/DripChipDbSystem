using DripChipDbSystem.Api.Controllers.Common.Attributes;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [HttpGet("/accounts/{accountId}")]
        [ProducesResponseType(typeof(AccountResponseContract), 200)]
        [ProducesResponseType(typeof(AccountResponseContract), 400)]
        [ProducesResponseType(typeof(AccountResponseContract), 401)]
        [ProducesResponseType(typeof(AccountResponseContract), 404)]
        public async Task<IActionResult> GetAccountAsync([AccountId(typeof(AccountResponseContract))] int accountId)
        {
            var response = await _accountService.GetAccountAsync(accountId);
            return Ok(response);
        }
        
        /// <summary>
        /// Поиск аккаунтов пользователей по параметрам
        /// </summary>
        [HttpGet("/accounts/search")]
        [ProducesResponseType(typeof(IEnumerable<AccountResponseContract>), 200)]
        [ProducesResponseType(typeof(IEnumerable<AccountResponseContract>), 400)]
        [ProducesResponseType(typeof(IEnumerable<AccountResponseContract>), 401)]
        public async Task<IActionResult> SearchAsync(
            [FromQuery] string firstName,
            [FromQuery] string lastName,
            [FromQuery] string email,
            [FromQuery] int from,
            [FromQuery] int size
            )
        {
            var response = await _accountService.SearchAsync(firstName, lastName, email, from, size);
            return Ok(response);
        }

        /// <summary>
        /// Обновление данных аккаунта пользователя
        /// </summary>
        [HttpPut("/accounts/{accountId}")]
        [ProducesResponseType(typeof(AccountResponseContract), 200)]
        [ProducesResponseType(typeof(AccountResponseContract), 400)]
        [ProducesResponseType(typeof(AccountResponseContract), 401)]
        [ProducesResponseType(typeof(AccountResponseContract), 403)]
        [ProducesResponseType(typeof(AccountResponseContract), 409)]
        public async Task<IActionResult> UpdateAccountAsync(
            [AccountId(typeof(AccountResponseContract))] int accountId,
            AccountRequestContract request)
        {
            var response = await _accountService.UpdateAccountAsync(accountId, request);
            return Ok(response);
        }

        /// <summary>
        /// Удаление аккаунта пользователя
        /// </summary>
        [HttpDelete("/accounts/{accountId}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<IActionResult> DeleteAccountAsync([AccountId(typeof(AccountResponseContract))] int accountId)
        {
            await _accountService.DeleteAccountAsync(accountId);
            return Ok();
        }
    }
}
