using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Services.AccountService;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с аутентификацией
    /// </summary>
    public class AuthService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly AccountEnsureService _accountEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AuthService(DatabaseContext databaseContext,
            AccountEnsureService accountEnsureService)
        {
            _databaseContext = databaseContext;
            _accountEnsureService = accountEnsureService;
        }

        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        public async Task<AccountResponseContract> AddAccountAsync(AccountRequestContract contract)
        {
            await _accountEnsureService.EnsureAccountNotExistsAsync(contract);

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
