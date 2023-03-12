using DripChipDbSystem.Api.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using DripChipDbSystem.Authentification;
using DripChipDbSystem.Api.Common.ResponseTypes;
using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using DripChipDbSystem.Services.AnimalVisitedLocationService;

namespace DripChipDbSystem.Api.Controllers.AnimalVisitedLocation
{
    /// <summary>
    /// Точка локации, посещенная животным
    /// </summary>
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class AnimalVisitedLocationController : Controller
    {
        private readonly AnimalVisitedLocationService _animalService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalVisitedLocationController(AnimalVisitedLocationService animalService)
        {
            _animalService = animalService;
        }

        /// <summary>
        /// Просмотр точек локации, посещенных животным
        /// </summary>
        [HttpGet("/animals/{animalId:long}/locations")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AnimalVisitedLocationResponseContract>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> SearchAsync(
            [IdValidation] long animalId,
            [FromQuery] DateTimeOffset? startDateTime,
            [FromQuery] DateTimeOffset? endDateTime,
            [FromQuery][FromValidation] int? from,
            [FromQuery][SizeValidation] int? size
            )
        {
            var response = await _animalService.SearchAsync(
                animalId,
                startDateTime,
                endDateTime,
                from,
                size);
            return Ok(response);
        }

        /// <summary>
        /// Добавление точки локации, посещенной животным
        /// </summary>
        [HttpPost("/animals/{animalId:long}/locations/{pointId:long}")]
        [ProducesResponseType(typeof(AnimalResponseContract), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> AddLocationAsync(
            [IdValidation] long animalId,
            [IdValidation] long pointId)
        {
            var response = await _animalService.AddAnimalVisitedLocationAsync(animalId, pointId);
            return new Created201Result(response);
        }

        /// <summary>
        /// Изменение точки локации, посещенной животным
        /// </summary>
        [HttpPut("/animals/{animalId:long}/locations")]
        [ProducesResponseType(typeof(AnimalVisitedLocationResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> UpdateAnimalVisitedLocationAsync(
            [IdValidation] long animalId,
            [FromBody] AnimalVisitedLocationRequestContract contract)
        {
            var response = await _animalService.UpdateAnimalVisitedLocationAsync(animalId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление точки локации, посещенной животным
        /// </summary>
        [HttpDelete("/animals/{animalId:long}/locations/{visitedPointId:long}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<IActionResult> DeleteAnimalVisitedLocationAsync(
            [IdValidation] long animalId,
            [IdValidation] long visitedPointId)
        {
            await _animalService.DeleteAnimalVisitedLocationAsync(animalId, visitedPointId);
            return Ok();
        }
    }
}
