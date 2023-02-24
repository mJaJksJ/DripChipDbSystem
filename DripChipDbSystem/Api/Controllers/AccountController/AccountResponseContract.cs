using DripChipDbSystem.Database.Models.Auth;

namespace DripChipDbSystem.Api.Controllers.AccountController
{
    /// <summary>
    /// Ответ на запрос аккаунта
    /// </summary>
    public class AccountResponseContract
    {
        /// <summary>
        /// Идентификатор аккаунта пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountResponseContract(Account account)
        {
            Id = account.Id;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Email = account.Email;
        }
    }
}
