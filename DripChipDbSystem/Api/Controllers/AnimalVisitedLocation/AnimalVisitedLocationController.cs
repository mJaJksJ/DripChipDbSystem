using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using DripChipDbSystem.Authentification;

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
            [FromQuery] DateTime startDateTime,
            [FromQuery] DateTime endDateTime,
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
        /// Изменение точки локации, посещенной животным
        /// </summary>
        [HttpPut("/animals/{animalId:long}/locations")]
        [ProducesResponseType(typeof(AnimalVisitedLocationResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> UpdateAnimalVisitedLocationAsync(
            [IdValidation] long animalId,
            AnimalVisitedLocationRequestContract contract)
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
