namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
    /// <summary>
    /// Контракт запроса типа животного
    /// </summary>
    [AnimalTypeRequestValidation]
    public class AnimalTypeRequestContract
    {
        /// <summary>
        /// Тип животного
        /// </summary>
        public string Type { get; set; }
    }
}
