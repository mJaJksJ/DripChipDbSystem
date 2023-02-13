using System.Threading.Tasks;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DripChipDbSystem.Api.Controllers.AuthController
{
    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Регистрация нового аккаунта
        /// </summary>
        [HttpPost("/registration")]
        [ProducesResponseType(typeof(AuthResponsetContract), 201)]
        [ProducesResponseType(typeof(AuthResponsetContract), 400)]
        [ProducesResponseType(typeof(AuthResponsetContract), 403)]
        [ProducesResponseType(typeof(AuthResponsetContract), 409)]
        public async Task<IActionResult> RegistrationAsync(AuthRequestContract requestContract)
        {
            if ( /*уже авторизирован*/false)
            {
                return new ObjectResult(new AuthResponsetContract()) { StatusCode = StatusCodes.Status403Forbidden };
            }

            if (await _authService.IsAccountExistsAsync(requestContract.Email))
            {
                return Conflict(new AuthResponsetContract());
            }

            var response = await _authService.AddAccountAsync(requestContract);
            return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
