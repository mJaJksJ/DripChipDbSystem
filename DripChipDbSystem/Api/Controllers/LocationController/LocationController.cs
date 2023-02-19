using System;
using DripChipDbSystem.Api.Controllers.AccountController;
using System.Threading.Tasks;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Mvc;
using DripChipDbSystem.Api.Controllers.Common.Attributes;

namespace DripChipDbSystem.Api.Controllers.LocationController
{
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
        [ProducesResponseType(typeof(LocationResponseContract), 200)]
        [ProducesResponseType(typeof(LocationResponseContract), 400)]
        [ProducesResponseType(typeof(LocationResponseContract), 401)]
        [ProducesResponseType(typeof(LocationResponseContract), 404)]
        public async Task<IActionResult> GetLocationAsync([AccountId(typeof(LocationResponseContract))] int pointId)
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
        public async Task<IActionResult> AddLocationAsync(LocationRequestContract contract)
        {
            var response = await _locationService.AddLocationAsync(contract);
            return Ok(response);
        }
    }
}