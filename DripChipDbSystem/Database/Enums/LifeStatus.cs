using System.Runtime.Serialization;

namespace DripChipDbSystem.Database.Enums
{
    /// <summary>
    /// Жизненный статус животного
    /// </summary>
    public enum LifeStatus
    {
        /// <summary>
        /// Живой
        /// </summary>
        [EnumMember(Value = "ALIVE")]
        Alive = 0,

        /// <summary>
        /// Мертвый
        /// </summary>
        [EnumMember(Value = "DEAD")]
        Dead = 1,
    }
}
