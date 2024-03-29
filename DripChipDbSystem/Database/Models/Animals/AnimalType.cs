using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DripChipDbSystem.Database.Models.Animals
{
    /// <summary>
    /// Тип животного
    /// </summary>
    public class AnimalType : IEntityTypeConfiguration<AnimalType>
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип животного
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Связанные с типом животные
        /// </summary>
        public IEnumerable<Animal> RelatedAnimals { get; set; }

        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AnimalType> builder)
        {
            builder.ToTable("animal_type");
            builder.Property(x => x.Type).IsRequired();
            builder.HasKey(x => x.Id);
        }
    }
}
