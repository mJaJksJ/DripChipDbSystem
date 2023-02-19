using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Api.Controllers.AuthController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    public class AuthService
    {
        private readonly DatabaseContext _databaseContext;

        public AuthService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<AccountResponseContract> AddAccountAsync(AccountRequestContract contract)
        {
            var newAccount = new Account
            {
                FirstName = contract.FirstName,
                LastName = contract.LastName,
                Email = contract.Email,
                PasswordHash = contract.Password
            };

            await _databaseContext.AddAsync(newAccount);
            await _databaseContext.SaveChangesAsync();

            return new AccountResponseContract
            {
                Id = newAccount.Id,
                FirstName = newAccount.FirstName,
                LastName = newAccount.LastName,
                Email = newAccount.Email,
            };
        }
    }
}
