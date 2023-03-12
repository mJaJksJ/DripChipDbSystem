using System.Text.RegularExpressions;

namespace DripChipDbSystem.Api.Common
{
    /// <summary>
    /// Регулярные выражения
    /// </summary>
    public static class Regexes
    {
        /// <summary>
        /// Состоит только из пробельных символов
        /// </summary>
        public static Regex OnlySpaceSymbols { get; } = new(@"^\s*$");

        /// <summary>
        /// Електронная почта
        /// </summary>
        public static Regex Email { get; } = new(@"^[\w\.\-]+@([\w]+\.[\w]{2,4}){1}$");
    }
}
