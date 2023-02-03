using System.Runtime.Serialization;

namespace DripChipDbSystem.Database.Enums
{
    /// <summary>
    /// Жизненный статус животного
    /// </summary>
    public enum LifeStatus
    {
        [EnumMember(Value = "ALIVE")]
        Alive = 0,

        [EnumMember(Value = "DEAD")]
        Dead = 1,
    }
}
