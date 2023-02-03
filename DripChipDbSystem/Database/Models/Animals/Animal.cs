using System;
using System.Collections.Generic;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Auth;

namespace DripChipDbSystem.Database.Models.Animals
{
    /// <summary>
    /// Животное
    /// </summary>
    public class Animal
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Типы животного
        /// </summary>
        public IEnumerable<AnimalType> AnimalType { get; set; }

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

        /// <inheritdoc cref="Enums.Gender"/>
        public Gender Gender { get; set; }

        /// <inheritdoc cref="Enums.LifeStatus"/>
        public LifeStatus LifeStatus { get; set; }

        /// <summary>
        /// Дата и время чипирования
        /// </summary>
        public DateTime ChippingDateTime { get; set; }

        /// <summary>
        /// Аккаунта чиппера
        /// </summary>
        public Account Chipper { get; set; }

        /// <summary>
        /// Id <see cref="Chipper"/>
        /// </summary>
        public int ChipperId { get; set; }

        /// <summary>
        /// Точка локации животного
        /// </summary>
        public LocationPoint ChippingLocationPoint { get; set; }

        /// <summary>
        /// Id <see cref="ChippingLocationPoint"/>
        /// </summary>
        public long ChippingLocationId { get; set; }

        /// <summary>
        /// Посещенные точки локации
        /// </summary>
        public IEnumerable<AnimalVisitedLocation> VisitedLocations { get; set; }

        /// <summary>
        /// Дата и время смерти
        /// </summary>
        public DateTime? DeathDateTime { get; set; }
    }
}
