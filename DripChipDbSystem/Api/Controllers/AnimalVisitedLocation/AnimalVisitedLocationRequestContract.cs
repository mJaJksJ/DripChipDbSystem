namespace DripChipDbSystem.Api.Controllers.AnimalVisitedLocation
{
    /// <summary>
    /// Контракт запроса посещенной животным точки локации
    /// </summary>
    [AnimalVisitedLocationRequestValidation]
    public class AnimalVisitedLocationRequestContract
    {
        /// <summary>
        /// Идентификатор объекта с информацией о посещенной точке локации
        /// </summary>
        public long? VisitedLocationPointId { get; set; }

        /// <summary>
        /// Идентификатор посещенной точки локации
        /// </summary>
        public long? LocationPointId { get; set; }
    }
}
