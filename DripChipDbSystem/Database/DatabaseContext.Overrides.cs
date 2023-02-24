using System.Threading;
using System.Threading.Tasks;

namespace DripChipDbSystem.Database
{
    public partial class DatabaseContext
    {
        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            TrackForSaveDetaches();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            TrackForSaveDetaches();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <inheritdoc/>
        public override int SaveChanges()
        {
            TrackForSaveDetaches();
            return base.SaveChanges();
        }

        /// <inheritdoc/>
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
