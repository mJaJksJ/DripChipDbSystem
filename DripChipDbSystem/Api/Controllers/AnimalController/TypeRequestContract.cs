using DripChipDbSystem.Api.Common.Attributes;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    /// <summary>
    /// Контракт запроса типа животного
    /// </summary>
    public class TypeRequestContract
    {
        /// <summary>
        /// Идентификатор текущего типа животного
        /// </summary>
        [IdValidation] public long OldTypeId { get; set; }

        /// <summary>
        /// Идентификатор нового типа животного для замены
        /// </summary>
        [IdValidation] public long NewTypeId { get; set; }
    }
}
