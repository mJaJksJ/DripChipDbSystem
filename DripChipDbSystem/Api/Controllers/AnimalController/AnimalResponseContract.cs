using System.Collections.Generic;
using System;
using System.Linq;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Utils;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    /// <summary>
    /// Ответ на запрос животного
    /// </summary>
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
        public string ChippingDateTime { get; set; }

        /// <summary>
        /// Идентификатор аккаунта чиппера
        /// </summary>
        public int ChipperId { get; set; }


        /// <summary>
        /// Идентификатор точки локации животных
        /// </summary>
        public long ChippingLocationId { get; set; }

        /// <summary>
        /// Массив идентификаторов объектов с информацией о посещенных точках локаций
        /// </summary>
        public IEnumerable<long> VisitedLocations { get; set; }

        /// <summary>
        /// Дата и время смерти
        /// </summary>
        public string DeathDateTime { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalResponseContract(Animal animal)
        {
            Id = animal.Id;
            AnimalTypes = animal.AnimalTypes?.Select(x => x.Id) ?? new long[] { 0 };
            Weight = animal.Weight;
            Length = animal.Length;
            Height = animal.Height;
            Gender = animal.Gender.GetMemberValue();
            LifeStatus = animal.LifeStatus.GetMemberValue();
            ChippingDateTime = animal.ChippingDateTime.ToString("O");
            ChipperId = animal.ChipperId;
            ChippingLocationId = animal.ChippingLocationPointId;
            VisitedLocations = animal.VisitedLocations?.Select(x => x.Id) ?? new long[] { 0 };
            DeathDateTime = animal.DeathDateTime?.ToString("O");
        }
    }
}
