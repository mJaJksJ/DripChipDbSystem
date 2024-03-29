using System;
using System.Linq;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DripChipDbSystem.Database
{
    /// <summary>
    /// Расширения для трекера изменений
    /// </summary>
    public static class ChangeTrackerExtensions
    {
        /// <summary>
        /// Обработка изменений <see cref="Animal"/>
        /// </summary>
        public static void AnimalTracker(this ChangeTracker changeTracker)
        {
            foreach (var item in changeTracker.Entries<Animal>()
                .Where(e => e.State == EntityState.Modified))
            {
                if (item.Entity.LifeStatus == LifeStatus.Dead)
                {
                    item.CurrentValues[nameof(Animal.DeathDateTime)] = DateTimeOffset.Now;
                }
            }

            if (changeTracker
                .Entries<Animal>()
                .Where(e => e.State == EntityState.Added)
                .Any(item => item.Entity.LifeStatus == LifeStatus.Dead))
            {
                throw new DripChipDbSystemException("Нельзя добавить мертвое животное");
            }
        }
    }
}
