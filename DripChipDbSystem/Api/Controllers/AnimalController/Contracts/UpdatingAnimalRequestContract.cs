using DripChipDbSystem.Api.Controllers.AnimalController.Attributes;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Contracts
{
    /// <summary>
    /// <inheritdoc cref="AnimalRequestContract"/>
    /// <remarks>Обновление животного</remarks>
    /// </summary>
    [UpdatingingAnimalRequestValidation]
    public class UpdatingAnimalRequestContract : AnimalRequestContract
    {
        /// <summary>
        /// Жизненный статус животного, доступные значения
        /// “ALIVE”(устанавливается автоматически при добавлении нового животного),
        /// “DEAD”(можно установить при обновлении информации о животном)
        /// </summary>
        public string LifeStatus { get; set; }
    }
}
