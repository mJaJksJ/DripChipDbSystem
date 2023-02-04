using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace DripChipDbSystem.Database
{
    public partial class DatabaseContext
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            TrackForSaveDetaches();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            TrackForSaveDetaches();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            TrackForSaveDetaches();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            TrackForSaveDetaches();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void TrackForSaveDetaches()
        {
            ChangeTracker.DetectChanges();
            ChangeTracker.AnimalTracker();
        }
    }
}
