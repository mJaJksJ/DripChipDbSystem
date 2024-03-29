using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AccountService
{
    /// <summary>
    /// Сервис работы с аккаунтамип
    /// </summary>
    public class AccountService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly AccountEnsureService _accountEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountService(
            DatabaseContext databaseContext,
            AccountEnsureService accountEnsureService)
        {
            _databaseContext = databaseContext;
            _accountEnsureService = accountEnsureService;
        }

        /// <summary>
        /// Получение информации об аккаунте пользователя
        /// </summary>
        public async Task<AccountResponseContract> GetAccountAsync(int accountId)
        {
            var account = await _accountEnsureService.EnsureAccountExistsAsync(accountId);
            return new AccountResponseContract(account);
        }

        /// <summary>
        /// Поиск аккаунтов пользователей по параметрам
        /// </summary>
        public async Task<IEnumerable<AccountResponseContract>> SearchAsync(
            string firstName,
            string lastName,
            string email,
            int? from,
            int? size)
        {
            var accounts = await _databaseContext.Accounts
                .AsNoTracking()
                .Where(x =>
                    firstName != null &&
                    x.FirstName.ToLower().Contains(firstName.ToLower()) || firstName == null)
                .Where(x =>
                    lastName != null &&
                    x.LastName.ToLower().Contains(lastName.ToLower()) || lastName == null)
                .Where(x =>
                    email != null &&
                    x.Email.ToLower().Contains(email.ToLower()) || email == null)
                .OrderBy(x => x.Id)
                .Skip(from ?? 0)
                .Take(size ?? 10)
                .ToListAsync();

            return accounts.Select(x => new AccountResponseContract(x));
        }

        /// <summary>
        /// Обновление данных аккаунта пользователя
        /// </summary>
        public async Task<AccountResponseContract> UpdateAccountAsync(
            int accountId,
            AccountRequestContract contract,
            int? userId)
        {
            var account = await _accountEnsureService.EnsureAccountExistsAsync(accountId, userId);

            account.FirstName = contract.FirstName;
            account.LastName = contract.LastName;
            account.Email = contract.Email;
            account.PasswordHash = contract.Password;

            await _databaseContext.SaveChangesAsync();
            return new AccountResponseContract(account);
        }

        /// <summary>
        /// Удаление аккаунта пользователя
        /// </summary>
        public async Task DeleteAccountAsync(int accountId, int? userId)
        {
            await _accountEnsureService.EnsureAccountHasNoAnimalsAsync(accountId);
            var account = await _accountEnsureService.EnsureAccountExistsAsync(accountId, userId);
            _databaseContext.Remove(account);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
