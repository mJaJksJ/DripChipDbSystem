using System;

namespace DripChipDbSystem.Api.Controllers.AnimalVisitedLocation
{
    public class AnimalVisitedLocationResponseContract
    {
        /// <summary>
        /// Идентификатор объекта с информацией о посещенной точке локации
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата и время посещения животным точки локации
        /// </summary>
        public DateTime DateTimeOfVisitLocationPoint { get; set; }

        /// <summary>
        /// Идентификатор посещенной точки локации
        /// </summary>
        public long LocationPointId { get; set; }
    }
}
