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
        public static string ToIsoString(this DateTimeOffset dateTime, bool withoutMilliSeconds = true)
        {
            return (withoutMilliSeconds
                    ? new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Offset)
                    : dateTime)
                .ToString("O");
        }
    }
}
