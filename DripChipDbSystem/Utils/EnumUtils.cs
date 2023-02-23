using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DripChipDbSystem.Utils
{
    public static class EnumUtils
    {
        public static T TryGetCustomAttribute<T>(this Enum value)
            where T : Attribute
        {
            var stringValue = value.ToString();

            return value
                .GetType()
                .GetMember(stringValue)
                .FirstOrDefault()
                ?.GetCustomAttribute<T>();
        }

        public static string GetMemberValue(this Enum value)
        {
            return value.TryGetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
        }

        public static T GetEnumValueByMemberValue<T>(this string memberValue)
            where T : Enum
        {
            return GetValues<T>().FirstOrDefault(x => x.GetMemberValue() == memberValue) ??
                   throw new InvalidEnumArgumentException(memberValue);
        }

        public static IEnumerable<TEnum> GetValues<TEnum>()
            where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>();
        }
    }
}
