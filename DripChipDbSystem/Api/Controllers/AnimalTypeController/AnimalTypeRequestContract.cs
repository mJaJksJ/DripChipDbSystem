namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
    [AnimalTypeValidation(typeof(AnimalTypeRequestContract))]
    public class AnimalTypeRequestContract
    {
        /// <summary>
        /// Тип животного
        /// </summary>
        public string Type { get; set; }
    }
}
