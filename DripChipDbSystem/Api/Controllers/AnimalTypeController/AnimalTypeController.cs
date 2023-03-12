using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Api.Common.ResponseTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DripChipDbSystem.Authentification;
using DripChipDbSystem.Services.AnimalTypeService;

namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
    /// <summary>
    /// Типы животных
    /// </summary>
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class AnimalTypeController : Controller
    {
        private readonly AnimalTypeService _animalTypeService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalTypeController(AnimalTypeService animalTypeService)
        {
            _animalTypeService = animalTypeService;
        }

        /// <summary>
        /// Получение информации о типе животного
        /// </summary>
        [HttpGet("/animals/types/{typeId:long}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAnimalTypeAsync([IdValidation] long typeId)
        {
            var response = await _animalTypeService.GetAnimalTypeAsync(typeId);
            return Ok(response);
        }

        /// <summary>
        /// Добавление типа животного
        /// </summary>
        [HttpPost("/animals/types")]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> AddAnimalTypeAsync([FromBody] AnimalTypeRequestContract contract)
        {
            var response = await _animalTypeService.AddAnimalTypeAsync(contract);
            return new Created201Result(response);
        }

        /// <summary>
        /// Изменение типа животного
        /// </summary>
        [HttpPut("/animals/types/{typeId:long}")]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        public async Task<IActionResult> UpdateAnimalTypeAsync(
            [IdValidation] long typeId,
            [FromBody] AnimalTypeRequestContract contract)
        {
            var response = await _animalTypeService.UpdateAnimalTypeAsync(typeId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление типа животного
        /// </summary>
        [HttpDelete("/animals/types/{typeId:long}")]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> DeleteAnimalTypeAsync([IdValidation] long typeId)
        {
            await _animalTypeService.DeleteAnimalTypeAsync(typeId);
            return Ok();
        }
    }
}
