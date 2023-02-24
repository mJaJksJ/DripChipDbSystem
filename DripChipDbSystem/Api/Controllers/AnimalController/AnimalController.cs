using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
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

        public AnimalController(AnimalService animalService)
        {
            _animalService = animalService;
        }

        /// <summary>
        /// Получение информации об аккаунте пользователя
        /// </summary>
        [HttpGet("/animals/{animalId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AnimalResponseContract), 200)]
        [ProducesResponseType(typeof(AnimalResponseContract), 400)]
        [ProducesResponseType(typeof(AnimalResponseContract), 401)]
        [ProducesResponseType(typeof(AnimalResponseContract), 404)]
        public async Task<IActionResult> GetAnimalAsync([IdValidation(typeof(AnimalResponseContract))] int animalId)
        {
            var response = await _animalService.GetAnimalAsync(animalId);
            return Ok(response);
        }

        /// <summary>
        /// Поиск аккаунтов пользователей по параметрам
        /// </summary>
        [HttpGet("/animals/search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<AnimalResponseContract>), 200)]
        [ProducesResponseType(typeof(IEnumerable<AnimalResponseContract>), 400)]
        [ProducesResponseType(typeof(IEnumerable<AnimalResponseContract>), 401)]
        public async Task<IActionResult> SearchAsync(
            [FromQuery] DateTime startDateTime,
            [FromQuery] DateTime endDateTime,
            [FromQuery] int chipperId,
            [FromQuery] long chippingLocationId,
            [FromQuery] string lifeStatus,
            [FromQuery] string gender,
            [FromQuery][FromValidation(typeof(AnimalResponseContract))] int? from,
            [FromQuery][SizeValidation(typeof(AnimalResponseContract))] int? size
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
        /// Обновление данных аккаунта пользователя
        /// </summary>
        [HttpPut("/animals/{animalId}")]
        [ProducesResponseType(typeof(AnimalResponseContract), 200)]
        [ProducesResponseType(typeof(AnimalResponseContract), 400)]
        [ProducesResponseType(typeof(AnimalResponseContract), 401)]
        [ProducesResponseType(typeof(AnimalResponseContract), 403)]
        [ProducesResponseType(typeof(AnimalResponseContract), 409)]
        public async Task<IActionResult> UpdateAnimalAsync(
            [IdValidation(typeof(AnimalResponseContract))] int animalId,
            AnimalRequestContract contract)
        {
            var response = await _animalService.UpdateAnimalAsync(animalId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление аккаунта пользователя
        /// </summary>
        [HttpDelete("/animals/{animalId}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<IActionResult> DeleteAnimalAsync([IdValidation(typeof(AnimalResponseContract))] int animalId)
        {
            await _animalService.DeleteAnimalAsync(animalId);
            return Ok();
        }

        /// <summary>
        /// Добавление типа животного к животному
        /// </summary>
        [HttpPost("/animals/{animalId}/types/{typeId}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> AddAnimalType(
            [IdValidation(typeof(AnimalResponseContract))] long animalId,
            [IdValidation(typeof(AnimalResponseContract))] long typeId)
        {
            var response = await _animalService.AddAnimalTypeAsync(animalId, typeId);
            return Ok(response);
        }

        /// <summary>
        ///  Изменение типа животного у животного
        /// </summary>
        [HttpPost("/animals/{animalId}/types")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> UpdatenimalType(
            [IdValidation(typeof(AnimalResponseContract))] long animalId,
            TypeRequestContract contract)
        {
            var response = await _animalService.UpdateAnimalTypeAsync(animalId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Добавление типа животного к животному
        /// </summary>
        [HttpDelete("/animals/{animalId}/types/{typeId}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> DeleteAnimalType(
            [IdValidation(typeof(AnimalResponseContract))] long animalId,
            [IdValidation(typeof(AnimalResponseContract))] long typeId)
        {
            var response = await _animalService.AddAnimalTypeAsync(animalId, typeId);
            return Ok(response);
        }
    }
}
