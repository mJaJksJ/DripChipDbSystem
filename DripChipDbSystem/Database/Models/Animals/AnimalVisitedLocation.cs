using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DripChipDbSystem.Database.Models.Animals
{
    public class AnimalVisitedLocation : IEntityTypeConfiguration<AnimalVisitedLocation>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <inheritdoc cref="Animals.Animal"/>
        public Animal Animal { get; set; }

        /// <summary>
        /// Id <see cref="Animal"/>
        /// </summary>
        public long AnimalId { get; set; }

        /// <inheritdoc cref="Animals.LocationPoint"/>
        public LocationPoint LocationPoint { get; set; }

        /// <summary>
        /// Id <see cref="LocationPoint"/>
        /// </summary>
        public long LocationPointId { get; set; }

        /// <summary>
        /// Дата и время посещения
        /// </summary>
        public DateTime VisitedDateTime { get; set; }

        public void Configure(EntityTypeBuilder<AnimalVisitedLocation> builder)
        {
            builder.ToTable("animal_visited_location");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Animal)
                .WithMany(x => x.VisitedLocations)
                .HasForeignKey(x => x.AnimalId);
            builder.HasOne(x => x.LocationPoint);
        }
    }
}
