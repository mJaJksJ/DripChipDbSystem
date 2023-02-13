using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Auth;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    public class AccountService
    {
        private readonly DatabaseContext _databaseContext;

        public AccountService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Account> GetAccountAsync(int accountId)
        {
            var account = await _databaseContext.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == accountId);

            if (account is null)
            {
                throw new NotFound404Exception();
            }

            return account;
        }
    }
}
