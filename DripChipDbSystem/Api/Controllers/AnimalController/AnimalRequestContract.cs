using System.Collections.Generic;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    [AnimalRequestValidation(typeof(AnimalRequestContract))]
    public class AnimalRequestContract
    {
        /// <summary>
        /// Массив идентификаторов типов животного
        /// </summary>
        public IEnumerable<long?> AnimalTypes { get; set; }

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
