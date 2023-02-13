namespace DripChipDbSystem.Api.Controllers.AuthController
{
    /// <summary>
    /// Контракт запроса при авторизации
    /// </summary>
    public class AuthResponsetContract
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
    }
}
