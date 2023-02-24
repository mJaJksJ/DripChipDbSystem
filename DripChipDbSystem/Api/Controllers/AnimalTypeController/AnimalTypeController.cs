using DripChipDbSystem.Services;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DripChipDbSystem.Authentification;

namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
    [Authorize(AuthenticationSchemes = BasicAuth.Scheme)]
    public class AnimalTypeController : Controller
    {
        private readonly AnimalTypeService _animalTypeService;

        public AnimalTypeController(AnimalTypeService animalTypeService)
        {
            _animalTypeService = animalTypeService;
        }

        /// <summary>
        /// Получение информации о точке локации животных
        /// </summary>
        [HttpGet("/animals/types/{typeId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 400)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 401)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 404)]
        public async Task<IActionResult> GetAnimalTypeAsync([IdValidation(typeof(AnimalTypeResponseContract))] long typeId)
        {
            var response = await _animalTypeService.GetAnimalTypeAsync(typeId);
            return Ok(response);
        }

        /// <summary>
        /// Добавление точки локации животных
        /// </summary>
        [HttpPost("/animals/types")]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 400)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 401)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 409)]
        public async Task<IActionResult> AddAnimalTypeAsync(AnimalTypeRequestContract contract)
        {
            await _animalTypeService.EnsureAnimalTypeNotExists(contract);
            var response = await _animalTypeService.AddAnimalTypeAsync(contract);
            return Ok(response);
        }

        /// <summary>
        /// Изменение точки локации животных
        /// </summary>
        [HttpPut("/animals/types/{typeId}")]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 400)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 401)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 404)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 409)]
        public async Task<IActionResult> UpdateAnimalTypeAsync([IdValidation(typeof(AnimalTypeResponseContract))] long typeId, AnimalTypeRequestContract contract)
        {
            await _animalTypeService.EnsureAnimalTypeNotExists(contract);
            var response = await _animalTypeService.UpdateAnimalTypeAsync(typeId, contract);
            return Ok(response);
        }

        /// <summary>
        /// Удаление точки локации животных 
        /// </summary>
        [HttpDelete("/animals/types/{typeId}")]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 200)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 400)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 401)]
        [ProducesResponseType(typeof(AnimalTypeResponseContract), 404)]
        public async Task<IActionResult> DeleteAnimalTypeAsync([IdValidation(typeof(AnimalTypeResponseContract))] long typeId)
        {
            await _animalTypeService.DeleteAnimalTypeAsync(typeId);
            return Ok();
        }
    }
}
