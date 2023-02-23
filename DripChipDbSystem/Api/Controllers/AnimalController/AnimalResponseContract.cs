using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Database.Models.Auth;
using System.Collections.Generic;
using System;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    public class AnimalResponseContract
    {
        /// <summary>
        /// Идентификатор животного
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Массив идентификаторов типов животного
        /// </summary>
        public IEnumerable<long> AnimalTypes { get; set; }

        /// <summary>
        /// Масса животного, кг
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Длина животного, м
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Высота животного, м
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Гендерный признак животного, доступные значения “MALE”, “FEMALE”, “OTHER”

        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Жизненный статус животного, доступные значения “ALIVE”, “DEAD”
        /// </summary>
        public string LifeStatus { get; set; }

        /// <summary>
        /// Дата и время чипирования
        /// </summary>
        public DateTime ChippingDateTime { get; set; }

        /// <summary>
        /// Идентификатор аккаунта чиппера
        /// </summary>
        public int ChipperId { get; set; }


        /// <summary>
        /// Идентификатор точки локации животных
        /// </summary>
        public long ChippingLocationPointId { get; set; }

        /// <summary>
        /// Массив идентификаторов объектов с информацией о посещенных точках локаций
        /// </summary>
        public IEnumerable<long> VisitedLocations { get; set; }

        /// <summary>
        /// Дата и время смерти
        /// </summary>
        public DateTime DeathDateTime { get; set; }
    }
}
