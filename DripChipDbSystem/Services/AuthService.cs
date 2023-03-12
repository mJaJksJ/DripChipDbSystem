using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Auth;
using DripChipDbSystem.Services.Account;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с аутентификацией
    /// </summary>
    public class AuthService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly AccountService _accountService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AuthService(DatabaseContext databaseContext,
            AccountService accountService)
        {
            _databaseContext = databaseContext;
            _accountService = accountService;
        }

        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        public async Task<AccountResponseContract> AddAccountAsync(AccountRequestContract contract)
        {
            await _accountService.EnsureAccountNotExists(contract);

            var newAccount = new Database.Models.Auth.Account
            {
                FirstName = contract.FirstName,
                LastName = contract.LastName,
                Email = contract.Email,
                PasswordHash = contract.Password
            };

            await _databaseContext.AddAsync(newAccount);
            await _databaseContext.SaveChangesAsync();

            return new AccountResponseContract(newAccount);
        }
    }
}
