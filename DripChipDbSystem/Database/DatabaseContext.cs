using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Database.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Database
{
    public partial class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<AnimalVisitedLocation> AnimalVisitedLocations { get; set; }
        public DbSet<LocationPoint> LocationPoints { get; set; }
    }
}
