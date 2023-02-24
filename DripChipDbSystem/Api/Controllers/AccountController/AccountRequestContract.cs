namespace DripChipDbSystem.Api.Controllers.AccountController
{
    /// <summary>
    /// Контракт запроса аккаунта
    /// </summary>
    [AccountRequestValidation]
    public class AccountRequestContract
    {
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
        /// Пароль от аккаунта
        /// </summary>
        public string Password { get; set; }
    }
}
