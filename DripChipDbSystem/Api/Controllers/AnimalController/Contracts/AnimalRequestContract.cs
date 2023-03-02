using DripChipDbSystem.Api.Controllers.AnimalController.Attributes;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Contracts
{
    /// <summary>
    /// Контракт запроса животного
    /// </summary>
    [AnimalRequestValidation]
    public class AnimalRequestContract
    {
        /// <summary>
        /// Масса животного, кг
        /// </summary>
        public float? Weight { get; set; }

        /// <summary>
        /// Длина животного, м
        /// </summary>
        public float? Length { get; set; }

        /// <summary>
        /// Высота животного, м
        /// </summary>
        public float? Height { get; set; }

        /// <summary>
        /// Гендерный признак животного, доступные значения “MALE”, “FEMALE”, “OTHER”
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Идентификатор аккаунта чиппера
        /// </summary>
        public int? ChipperId { get; set; }

        /// <summary>
        /// Идентификатор точки локации животных
        /// </summary>
        public long? ChippingLocationId { get; set; }
    }
}
