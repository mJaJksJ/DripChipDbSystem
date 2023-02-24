using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Authentification;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChipDbSystem.Api.Controllers.LocationController
{
    /// <summary>
    /// Точка локации животных
    /// </summary>
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;

        /// <summary>
        /// .ctor
        /// </summary>
        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Получение информации о точке локации животных
        /// </summary>
        [HttpGet("/locations/{pointId:long}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetLocationAsync([IdValidation] long pointId)
        {
            var response = await _locationService.GetLocationAsync(pointId);
            return Ok(response);
        }

        /// <summary>
        /// Добавление точки локации животных
        /// </summary>
        [HttpPost("/locations")]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> AddLocationAsync([FromBody] LocationRequestContract contract)
        {
            var response = await _locationService.AddLocationAsync(contract);
            return Ok(response);
        }

        /// <summary>
        /// Изменение точки локации животных
        /// </summary>
        [HttpPut("/locations/{pointId:long}")]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> UpdateLocationAsync(
            [IdValidation] long pointId,
            [FromBody] LocationRequestContract contract)
        {
            var response = await _locationService.UpdateLocationAsync(pointId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление точки локации животных 
        /// </summary>
        [HttpDelete("/locations/{pointId:long}")]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> DeleteLocationAsync([IdValidation] long pointId)
        {
            await _locationService.DeleteLocationAsync(pointId);
            return Ok();
        }
    }
}