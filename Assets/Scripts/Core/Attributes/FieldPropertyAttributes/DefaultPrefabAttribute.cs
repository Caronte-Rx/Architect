using System;
using System.Reflection;
using UObject = UnityEngine.Object;

namespace Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DefaultPrefabAttribute : Attribute
    {
        public string PrefabName
            { get; private set; }

        public Type Type
            { get; private set; }

        public DefaultPrefabAttribute(Type type)
            : this(type, type.Name) { }

        public DefaultPrefabAttribute(Type type, string prefabName)
        {
            PrefabName  = prefabName; 
            Type        = type;
        }

        public static UObject GetPrefab(MemberInfo member)
            => AttributeUtility.GetAttribute<DefaultPrefabAttribute>(member)?.LoadPrefab();

        private UObject LoadPrefab()
            => AssetUtility.LoadPrefab(Type, PrefabName);
    }
}
