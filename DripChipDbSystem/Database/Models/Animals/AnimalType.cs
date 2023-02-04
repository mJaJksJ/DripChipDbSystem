﻿using Microsoft.EntityFrameworkCore;
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

        public void Configure(EntityTypeBuilder<AnimalType> builder)
        {
            builder.ToTable("animal_type");
            builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(Type)}", $"[{nameof(Type)}] NOT NULL]"));
            builder.HasKey(x => x.Id);
        }
    }
}