using System.Threading.Tasks;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.Account
{
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
        public async Task<Database.Models.Auth.Account> EnsureAccountExists(int id, int? userId)
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
        public async Task<Database.Models.Auth.Account> EnsureAccountExists(int id)
        {
            var account = await _databaseContext.Accounts
                .SingleOrDefaultAsync(x => x.Id == id);

            return  account ?? throw new NotFound404Exception();
        }

        public async Task EnsureAccountHasNoAnimals(int accountId)
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
