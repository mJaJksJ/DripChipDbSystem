using System;

namespace DripChipDbSystem.Utils
{
    /// <summary>
    /// Инструменты работы с датой и временем
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// ISO-8601
        /// </summary>
        public static string ToIsoString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString("O");
        }
    }
}
