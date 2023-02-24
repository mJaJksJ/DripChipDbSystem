using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DripChipDbSystem.Utils
{
    /// <summary>
    /// Инструменты работы с Enum
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Значение <see cref="EnumMemberAttribute"/>
        /// </summary>
        public static string GetMemberValue(this Enum value)
        {
            return value.TryGetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
        }

        /// <summary>
        /// Значение в зависимости от <see cref="EnumMemberAttribute"/>
        /// </summary>
        public static T GetEnumValueByMemberValue<T>(this string memberValue)
            where T : Enum
        {
            return GetValues<T>().FirstOrDefault(x => x.GetMemberValue() == memberValue) ??
                   throw new InvalidEnumArgumentException(memberValue);
        }

        /// <summary>
        /// Получить значения
        /// </summary>
        public static IEnumerable<TEnum> GetValues<TEnum>()
            where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>();
        }

        private static T TryGetCustomAttribute<T>(this Enum value)
            where T : Attribute
        {
            var stringValue = value.ToString();

            return value
                .GetType()
                .GetMember(stringValue)
                .FirstOrDefault()
                ?.GetCustomAttribute<T>();
        }
    }
}
