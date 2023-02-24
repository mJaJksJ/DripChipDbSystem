using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Api.Controllers.AnimalController;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using DripChipDbSystem.Authentification;

namespace DripChipDbSystem.Api.Controllers.AnimalVisitedLocation
{
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class AnimalVisitedLocationController : Controller
    {
        private readonly AnimalVisitedLocationService _animalService;

        public AnimalVisitedLocationController(AnimalVisitedLocationService animalService)
        {
            _animalService = animalService;
        }

        /// <summary>
        /// Поиск аккаунтов пользователей по параметрам
        /// </summary>
        [HttpGet("/animals/{animalId}/locations")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AnimalVisitedLocationResponseContract>), 200)]
        [ProducesResponseType(typeof(IEnumerable<AnimalVisitedLocationResponseContract>), 400)]
        [ProducesResponseType(typeof(IEnumerable<AnimalVisitedLocationResponseContract>), 401)]
        public async Task<IActionResult> SearchAsync(
            [IdValidation(typeof(AnimalResponseContract))] long animalId,
            [FromQuery] DateTime startDateTime,
            [FromQuery] DateTime endDateTime,
            [FromQuery][FromValidation(typeof(AnimalVisitedLocationResponseContract))] int? from,
            [FromQuery][SizeValidation(typeof(AnimalVisitedLocationResponseContract))] int? size
            )
        {
            var response = await _animalService.SearchAsync(
                animalId,
                startDateTime,
                endDateTime,
                from ?? 0,
                size ?? 10);
            return Ok(response);
        }

        /// <summary>
        /// Обновление данных аккаунта пользователя
        /// </summary>
        [HttpPut("/animals/{animalId}/locations")]
        [ProducesResponseType(typeof(AnimalVisitedLocationResponseContract), 200)]
        [ProducesResponseType(typeof(AnimalVisitedLocationResponseContract), 400)]
        [ProducesResponseType(typeof(AnimalVisitedLocationResponseContract), 401)]
        [ProducesResponseType(typeof(AnimalVisitedLocationResponseContract), 403)]
        [ProducesResponseType(typeof(AnimalVisitedLocationResponseContract), 409)]
        public async Task<IActionResult> UpdateAnimalVisitedLocationAsync(
            [IdValidation(typeof(AnimalVisitedLocationResponseContract))] int animalId,
            AnimalVisitedLocationRequestContract contract)
        {
            var response = await _animalService.UpdateAnimalVisitedLocationAsync(animalId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление аккаунта пользователя
        /// </summary>
        [HttpDelete("/animals/{animalId}/locations/{visitedPointId}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<IActionResult> DeleteAnimalVisitedLocationAsync(
            [IdValidation(typeof(AnimalVisitedLocationResponseContract))] long animalId,
            [IdValidation(typeof(AnimalVisitedLocationResponseContract))] long visitedPointId)
        {
            await _animalService.DeleteAnimalVisitedLocationAsync(animalId, visitedPointId);
            return Ok();
        }
    }
}
