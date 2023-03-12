using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Auth;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AccountService
{
    /// <summary>
    /// Сервис проверок
    /// </summary>
    public class AccountEnsureService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountEnsureService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Убедиться, что пользователь существует
        /// </summary>
        public async Task<Account> EnsureAccountExistsAsync(int id, int? userId)
        {
            var account = await _databaseContext.Accounts
                .SingleOrDefaultAsync(x => x.Id == id);

            return account is not null && account.Id == userId.GetValueOrDefault()
                ? account
                : throw new Forbidden403Exception();
        }

        /// <summary>
        /// Убедиться, что пользователь существует
        /// </summary>
        public async Task<Account> EnsureAccountExistsAsync(int id)
        {
            var account = await _databaseContext.Accounts
                .SingleOrDefaultAsync(x => x.Id == id);

            return account ?? throw new NotFound404Exception();
        }

        /// <summary>
        /// Убедиться, что пользователь не чипировал животных
        /// </summary>
        public async Task EnsureAccountHasNoAnimalsAsync(int accountId)
        {
            var animalsExist = await _databaseContext.Animals
                .AnyAsync(x => x.ChipperId == accountId);

            if (animalsExist)
            {
                throw new BadRequest400Exception();
            }
        }

        /// <summary>
        /// Убедиться, что пользователя не существует
        /// </summary>
        public async Task EnsureAccountNotExistsAsync(AccountRequestContract contract)
        {
            var isExists = await _databaseContext.Accounts
                .AnyAsync(x => x.Email == contract.Email);

            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }
    }
}
