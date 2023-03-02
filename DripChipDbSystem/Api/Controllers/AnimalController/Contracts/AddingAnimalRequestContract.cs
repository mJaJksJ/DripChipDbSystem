using System.Collections.Generic;
using DripChipDbSystem.Api.Controllers.AnimalController.Attributes;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Contracts
{
    /// <summary>
    /// <inheritdoc cref="AnimalRequestContract"/>
    /// <remarks>Добавление животного</remarks>
    /// </summary>
    [AddingAnimalRequestValidation]
    public class AddingAnimalRequestContract : AnimalRequestContract
    {
        /// <summary>
        /// Массив идентификаторов типов животного
        /// </summary>
        public IEnumerable<long?> AnimalTypes { get; set; }
    }
}
