using System;
using UnityEngine;

namespace Core 
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonSettingsAttribute : Attribute
    {
        public Type SettingsType
            { get; set; }

        public SingletonSettingsAttribute(Type settingsType)
        {
            SettingsType = settingsType;
        }

        public static IBaseSettings GetSingletonSettings<T>()
        {
            try
            {
                var settingsType =
                    AttributeUtility.GetAttribute<T, SingletonSettingsAttribute>()?.SettingsType;

                if (!typeof(IBaseSettings).IsAssignableFrom(settingsType))
                    throw new Exception(
                        $"The '{settingsType.Name}' is not a valid type. '{typeof(BaseSettings).Name}' type is required.");

                var parentType = settingsType;
                while (
                    !parentType.IsGenericType ||
                     parentType.GetGenericTypeDefinition() != typeof(SingletonSObject<>).GetGenericTypeDefinition())
                {
                    parentType = parentType.BaseType;
                }
                var property = parentType.GetProperty("SObject");
                if (property == null)
                    throw new Exception($"Unable to get the 'SObject' static property form '{settingsType.Name}'.");
                return (IBaseSettings)property.GetValue(null);
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to get the settings for '{typeof(T).Name}'. " +
                    $"Details: {ex.Message}");
            }
            return null;
        }
    }
}
