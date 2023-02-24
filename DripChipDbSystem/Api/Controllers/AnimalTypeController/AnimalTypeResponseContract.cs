using DripChipDbSystem.Database.Models.Animals;

namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
    /// <summary>
    /// Ответ на запрос типа животного
    /// </summary>
    public class AnimalTypeResponseContract
    {
        /// <summary>
        /// Идентификатор типа животного
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип животного
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalTypeResponseContract(AnimalType animalType)
        {
            Id = animalType.Id;
            Type = animalType.Type;
        }
    }
}
