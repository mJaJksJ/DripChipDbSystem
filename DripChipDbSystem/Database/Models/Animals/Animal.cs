using System;
using System.Collections.Generic;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DripChipDbSystem.Database.Models.Animals
{
    /// <summary>
    /// Животное
    /// </summary>
    public class Animal : IEntityTypeConfiguration<Animal>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Типы животного
        /// </summary>
        public IEnumerable<AnimalType> AnimalType { get; set; }

        /// <summary>
        /// Масса животного, кг
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Длина животного, м
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Высота животного, м
        /// </summary>
        public float Height { get; set; }

        /// <inheritdoc cref="Enums.Gender"/>
        public Gender Gender { get; set; }

        /// <inheritdoc cref="Enums.LifeStatus"/>
        public LifeStatus LifeStatus { get; set; }

        /// <summary>
        /// Дата и время чипирования
        /// </summary>
        public DateTime ChippingDateTime { get; set; }

        /// <summary>
        /// Аккаунта чиппера
        /// </summary>
        public Account Chipper { get; set; }

        /// <summary>
        /// Id <see cref="Chipper"/>
        /// </summary>
        public int ChipperId { get; set; }

        /// <summary>
        /// Точка локации животного
        /// </summary>
        public LocationPoint ChippingLocationPoint { get; set; }

        /// <summary>
        /// Id <see cref="ChippingLocationPoint"/>
        /// </summary>
        public long ChippingLocationPointId { get; set; }

        /// <summary>
        /// Посещенные точки локации
        /// </summary>
        public IEnumerable<AnimalVisitedLocation> VisitedLocations { get; set; }

        /// <summary>
        /// Дата и время смерти
        /// </summary>
        public DateTime? DeathDateTime { get; }

        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.ToTable("animal");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Chipper);
            builder.HasOne(x => x.ChippingLocationPoint);
            builder.HasMany(x => x.VisitedLocations)
                .WithOne(x => x.Animal)
                .HasForeignKey(x => x.AnimalId);
            builder.Property(x => x.LifeStatus)
                .HasDefaultValue(LifeStatus.Alive);
            builder.Property(x => x.DeathDateTime)
                .HasDefaultValue(null);

        }
    }
}