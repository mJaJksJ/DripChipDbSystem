namespace DripChipDbSystem.Api.Controllers.AuthControllers
{
    /// <summary>
    /// Контракт запроса при авторизации
    /// </summary>
    [AuthRequestValidation]
    public class AuthRequestContract
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
        /// Пароль от аккаунта пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
