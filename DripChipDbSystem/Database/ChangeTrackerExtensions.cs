using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

            foreach (var item in changeTracker.Entries<Animal>().Where(e => e.State == EntityState.Added))
            {
                if (item.Entity.LifeStatus == LifeStatus.Dead)
                {
                    throw new Exception("Нельзя добавить мертвое животное");
                }
            }
        }
    }
}
