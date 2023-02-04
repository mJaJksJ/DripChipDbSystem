using System;
using System.Linq;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DripChipDbSystem.Database
{
    public static class ChangeTrackerExtensions
    {
        public static void AnimalTracker(this ChangeTracker changeTracker)
        {
            foreach (var item in changeTracker.Entries<Animal>().Where(e => e.State == EntityState.Modified))
            {
                if (item.Entity.LifeStatus == LifeStatus.Dead)
                {
                    item.CurrentValues[nameof(Animal.DeathDateTime)] = DateTime.Now;
                }
            }

            if (changeTracker
                .Entries<Animal>()
                .Where(e => e.State == EntityState.Added)
                .Any(item => item.Entity.LifeStatus == LifeStatus.Dead))
            {
                throw new Exception("Нельзя добавить мертвое животное");
            }
        }
    }
}
