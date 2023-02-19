namespace DripChipDbSystem.Api.Controllers.AccountController
{
    [AccountRequestValidation(typeof(AccountRequestContract))]
    public class AccountRequestContract
    {
        /// <summary>
        /// Иимя пользователя
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

