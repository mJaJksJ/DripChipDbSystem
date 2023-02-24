using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Api.Common.ResponseTypes;
using DripChipDbSystem.Api.Controllers.LocationController;
using DripChipDbSystem.Authentification;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    /// <summary>
    /// Животное
    /// </summary>
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class AnimalController : Controller
    {
        private readonly AnimalService _animalService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalController(AnimalService animalService)
        {
            _animalService = animalService;
        }

        /// <summary>
        /// Получение информации о животном
        /// </summary>
        [HttpGet("/animals/{animalId:long}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AnimalResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAnimalAsync([IdValidation] long animalId)
        {
            var response = await _animalService.GetAnimalAsync(animalId);
            return Ok(response);
        }

        /// <summary>
        /// Поиск животных по параметрам
        /// </summary>
        [HttpGet("/animals/search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AnimalResponseContract>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> SearchAsync(
            [FromQuery] DateTime startDateTime,
            [FromQuery] DateTime endDateTime,
            [FromQuery] int chipperId,
            [FromQuery] long chippingLocationId,
            [FromQuery] string lifeStatus,
            [FromQuery] string gender,
            [FromQuery][FromValidation] int? from,
            [FromQuery][SizeValidation] int? size
            )
        {
            var response = await _animalService.SearchAsync(
                startDateTime,
                endDateTime,
                chipperId,
                chippingLocationId,
                lifeStatus,
                gender,
                from ?? 0,
                size ?? 10);
            return Ok(response);
        }

        /// <summary>
        /// Добавление нового животного 
        /// </summary>
        [HttpPost("/animals")]
        [ProducesResponseType(typeof(AnimalResponseContract), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> AddLocationAsync([FromBody] AnimalRequestContract contract)
        {
            var response = await _animalService.AddAnimalAsync(contract);
            return new Created201Result(response);
        }

        /// <summary>
        /// Обновление информации о животном 
        /// </summary>
        [HttpPut("/animals/{animalId:long}")]
        [ProducesResponseType(typeof(AnimalResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> UpdateAnimalAsync(
            [IdValidation] long animalId,
            AnimalRequestContract contract)
        {
            var response = await _animalService.UpdateAnimalAsync(animalId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление животного
        /// </summary>
        [HttpDelete("/animals/{animalId:long}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> DeleteAnimalAsync([IdValidation] long animalId)
        {
            await _animalService.DeleteAnimalAsync(animalId);
            return Ok();
        }

        /// <summary>
        /// Добавление типа животного к животному
        /// </summary>
        [HttpPost("/animals/{animalId:long}/types/{typeId:long}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> AddAnimalType(
            [IdValidation] long animalId,
            [IdValidation] long typeId)
        {
            var response = await _animalService.AddAnimalTypeAsync(animalId, typeId);
            return new Created201Result(response);
        }

        /// <summary>
        ///  Изменение типа животного у животного
        /// </summary>
        [HttpPost("/animals/{animalId:long}/types")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> UpdatenimalType(
            [IdValidation] long animalId,
            TypeRequestContract contract)
        {
            var response = await _animalService.UpdateAnimalTypeAsync(animalId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление типа животного у животного
        /// </summary>
        [HttpDelete("/animals/{animalId:long}/types/{typeId:long}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> DeleteAnimalType(
            [IdValidation] long animalId,
            [IdValidation] long typeId)
        {
            var response = await _animalService.DeleteAnimalTypeAsync(animalId, typeId);
            return Ok(response);
        }
    }
}
