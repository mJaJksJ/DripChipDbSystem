namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
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
    }
}
