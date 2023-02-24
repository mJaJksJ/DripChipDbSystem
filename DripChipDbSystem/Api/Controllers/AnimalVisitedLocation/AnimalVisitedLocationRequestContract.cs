using DripChipDbSystem.Api.Common.Attributes;

namespace DripChipDbSystem.Api.Controllers.AnimalVisitedLocation
{
    /// <summary>
    /// Контракт запроса посещенной животным точки локации
    /// </summary>
    public class AnimalVisitedLocationRequestContract
    {
        /// <summary>
        /// Идентификатор объекта с информацией о посещенной точке локации
        /// </summary>
        [IdValidation] public long VisitedLocationPointId { get; set; }

        /// <summary>
        /// Идентификатор посещенной точки локации
        /// </summary>
        [IdValidation] public long LocationPointId { get; set; }
    }
}
