using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Authentification;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChipDbSystem.Api.Controllers.LocationController
{
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Получение информации о точке локации животных
        /// </summary>
        [HttpGet("/locations/{pointId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(LocationResponseContract), 400)]
        [ProducesResponseType(typeof(LocationResponseContract), 401)]
        [ProducesResponseType(typeof(LocationResponseContract), 404)]
        public async Task<IActionResult> GetLocationAsync([IdValidation(typeof(LocationResponseContract))] long pointId)
        {
            var response = await _locationService.GetLocationAsync(pointId);
            return Ok(response);
        }

        /// <summary>
        /// Добавление точки локации животных
        /// </summary>
        [HttpPost("/locations")]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(LocationResponseContract), 400)]
        [ProducesResponseType(typeof(LocationResponseContract), 401)]
        [ProducesResponseType(typeof(LocationResponseContract), 409)]
        public async Task<IActionResult> AddLocationAsync([FromBody] LocationRequestContract contract)
        {
            await _locationService.EnsureLocationNotExists(contract);
            var response = await _locationService.AddLocationAsync(contract);
            return Ok(response);
        }

        /// <summary>
        /// Изменение точки локации животных
        /// </summary>
        [HttpPut("/locations/{pointId}")]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(LocationResponseContract), 400)]
        [ProducesResponseType(typeof(LocationResponseContract), 401)]
        [ProducesResponseType(typeof(LocationResponseContract), 404)]
        [ProducesResponseType(typeof(LocationResponseContract), 409)]
        public async Task<IActionResult> UpdateLocationAsync(
            [IdValidation(typeof(LocationResponseContract))] long pointId,
            [FromBody] LocationRequestContract contract)
        {
            await _locationService.EnsureLocationNotExists(contract);
            var response = await _locationService.UpdateLocationAsync(pointId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление точки локации животных 
        /// </summary>
        [HttpDelete("/locations/{pointId}")]
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(LocationResponseContract), 400)]
        [ProducesResponseType(typeof(LocationResponseContract), 401)]
        [ProducesResponseType(typeof(LocationResponseContract), 404)]
        public async Task<IActionResult> DeleteLocationAsync([IdValidation(typeof(LocationResponseContract))] long pointId)
        {
            await _locationService.DeleteLocationAsync(pointId);
            return Ok();
        }
    }
}