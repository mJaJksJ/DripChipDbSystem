using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DripChipDbSystem.Database.Models.Animals
{
    /// <summary>
    /// Точка локации
    /// </summary>
    public class LocationPoint : IEntityTypeConfiguration<LocationPoint>
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Географическая широта в градусах
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Географическая долгота в градусах
        /// </summary>
        public double Longitude { get; set; }
        
        public void Configure(EntityTypeBuilder<LocationPoint> builder)
        {
            builder.ToTable("location_point");
            builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(Latitude)}", $"[{nameof(Latitude)}] >= -90 AND [{nameof(Latitude)}] <= 90]"));
            builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(Longitude)}", $"[{nameof(Latitude)}] >= -180 AND [{nameof(Latitude)}] <= 180]"));
            builder.HasKey(x => x.Id);
        }
    }
}
