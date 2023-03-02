using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Database.Models.Auth;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с аккаунтамип
    /// </summary>
    public class AccountService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Получение информации об аккаунте пользователя
        /// </summary>
        public async Task<AccountResponseContract> GetAccountAsync(int accountId)
        {
            var account = await _databaseContext.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == accountId);

            return account is null
                ? throw new NotFound404Exception()
                : new AccountResponseContract(account);
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
            return (await _databaseContext.Accounts
                .AsNoTracking()
                .Where(x => firstName != null && x.FirstName.ToLower().Contains(firstName.ToLower()) || firstName == null)
                .Where(x => lastName != null && x.LastName.ToLower().Contains(lastName.ToLower()) || lastName == null)
                .Where(x => email != null && x.Email.ToLower().Contains(email.ToLower()) || email == null)
                .Skip(from ?? 0)
                .Take(size ?? 10)
                .OrderBy(x => x.Id)
                .ToListAsync())
                .Select(x => new AccountResponseContract(x));
        }

        /// <summary>
        /// Обновление данных аккаунта пользователя
        /// </summary>
        public async Task<AccountResponseContract> UpdateAccountAsync(
            int accountId,
            AccountRequestContract contract,
            int? userId)
        {
            var account = await EnsureAccountExists(accountId, userId);

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
            await EnsureAccountHasNoAnimals(accountId);
            var account = await EnsureAccountExists(accountId, userId);
            _databaseContext.Remove(account);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Убедиться, что пользователя не существует
        /// </summary>
        public async Task EnsureAccountNotExists(AccountRequestContract contract)
        {
            var isExists = await _databaseContext.Accounts
                .AnyAsync(x => x.Email == contract.Email);

            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }

        /// <summary>
        /// Убедиться, что пользователь существует
        /// </summary>
        public async Task<Account> EnsureAccountExists(int id, int? userId)
        {
            var account = await _databaseContext.Accounts
                .SingleOrDefaultAsync(x => x.Id == id);

            return account is not null && account.Id == userId.GetValueOrDefault()
                ? account
                : throw new Forbidden403Exception();
        }

        private async Task EnsureAccountHasNoAnimals(int accountId)
        {
            var animalsExist = await _databaseContext.Animals
                .AnyAsync(x => x.ChipperId == accountId);

            if (animalsExist)
            {
                throw new BadRequest400Exception();
            }
        }
    }
}
