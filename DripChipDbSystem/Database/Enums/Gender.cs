using System.Runtime.Serialization;

namespace DripChipDbSystem.Database.Enums
{
    /// <summary>
    /// Гендерный признак животного
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Муж.
        /// </summary>
        [EnumMember(Value = "MALE")]
        Male = 0,

        /// <summary>
        /// Жен.
        /// </summary>
        [EnumMember(Value = "FEMALE")]
        Female = 1,

        /// <summary>
        /// Другое
        /// </summary>
        [EnumMember(Value = "OTHER")]
        Other = 2,
    }
}
