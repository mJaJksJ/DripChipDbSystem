using System.Runtime.Serialization;

namespace DripChipDbSystem.Database.Enums
{
    /// <summary>
    /// Гендерный признак животного
    /// </summary>
    public enum Gender
    {
        [EnumMember(Value = "MALE")]
        Male = 0,

        [EnumMember(Value = "FEMALE")]
        Female = 1,

        [EnumMember(Value = "OTHER")]
        Other = 2,
    }
}
