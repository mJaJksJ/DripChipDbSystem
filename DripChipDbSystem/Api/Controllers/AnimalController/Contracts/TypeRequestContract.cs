using DripChipDbSystem.Api.Controllers.AnimalController.Attributes;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Contracts
{
    /// <summary>
    /// Контракт запроса типа животного
    /// </summary>
    [TypeRequestValidation]
    public class TypeRequestContract
    {
        /// <summary>
        /// Идентификатор текущего типа животного
        /// </summary>
        public long? OldTypeId { get; set; }

        /// <summary>
        /// Идентификатор нового типа животного для замены
        /// </summary>
        public long? NewTypeId { get; set; }
    }
}
