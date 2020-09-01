using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core
{
    public static class AttributeUtility
    {
        public static TAttribute GetAttribute<T, TAttribute>()
            where TAttribute : Attribute
            => GetAttribute<TAttribute>(typeof(T));

        public static TAttribute GetAttribute<TAttribute>(MemberInfo member)
            where TAttribute : Attribute
        {
            try
            {
                var attributes = member.GetCustomAttributes(typeof(TAttribute), true);
                if (attributes.Length == 0)
                    throw new Exception($"The attribute is missing.");
                return (TAttribute)attributes.First();
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to get the '{typeof(TAttribute).Name}' attribute form '{member.Name}'. " +
                    $"Details: {ex.Message}");
            }
            return null;
        }

        public static IEnumerable<FieldInfo> GetFields<T, TAttribute>()
            where TAttribute : Attribute 
            => GetFields<TAttribute>(typeof(T));

        public static IEnumerable<FieldInfo> GetFields<TAttribute>(Type type)
            where TAttribute : Attribute
            => type.GetFields().Where(field => field.IsDefined(typeof(TAttribute)));
    }
}
