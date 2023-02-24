using System;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Database.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.NameTranslation;

namespace DripChipDbSystem.Database
{
    /// <inheritdoc/>
    public partial class DatabaseContext : DbContext
    {
        /// <inheritdoc cref="Account"/>
        public DbSet<Account> Accounts { get; set; }
        /// <inheritdoc cref="Animal"/>
        public DbSet<Animal> Animals { get; set; }
        /// <inheritdoc cref="AnimalType"/>
        public DbSet<AnimalType> AnimalTypes { get; set; }
        /// <inheritdoc cref="AnimalVisitedLocation"/>
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations { get; set; }
        /// <inheritdoc cref="LocationPoint"/>
        public DbSet<LocationPoint> LocationPoints { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var storeObjectIdentifier = StoreObjectIdentifier.Table(entity.GetTableName() ?? string.Empty, entity.GetSchema());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(mapper.TranslateMemberName(property.GetColumnName(storeObjectIdentifier) ?? string.Empty));
                }
            }
        }
    }
}
