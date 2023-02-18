using System;
namespace DripChipDbSystem.Api.Controllers.AccountController
{
    [AccountRequestValidation(typeof(AccountRequestContract))]
    public class AccountRequestContract
    {
        /// <summary>
        /// Новое имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Новая фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Новый адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль от аккаунта
        /// </summary>
        public string Password { get; set; }
    }
}

