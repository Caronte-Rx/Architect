using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Core
{
    public static class AssetUtility
    {
        public static UObject LoadAsset(string filter, Type type, string[] searchInFolders, bool useExactName = true)
        {
            var assets = new List<UObject>();
            bool WasAssetFound()
            {
                if (assets.Count == 0)
                {
                    Debug.LogError(
                        $"There are no results for '{filter}' with type ({type.Name}) in " +
                        $"[{string.Join(", ", searchInFolders)}].");
                    return false;
                }
                if (assets.Count > 1)
                    Debug.LogWarning(
                        $"There is more than one result for '{filter}' with type ({type.Name}) in " +
                        $"[{string.Join(", ", searchInFolders)}].");
                return true;
            }
            bool NameMatches(string name)
                => !useExactName || (useExactName && filter.Contains(name));

            foreach (var guid in AssetDatabase.FindAssets(filter, searchInFolders))
            {
                var asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), type);
                if (asset != null && NameMatches(asset.name))
                    assets.Add(asset);
            }
            if (WasAssetFound())
                return assets[0];
            return null;
        }

        public static T LoadAssetByType<T>(string[] searchInFolders)
            => (T)Convert.ChangeType(LoadAssetByType(typeof(T), searchInFolders), typeof(T));

        public static UObject LoadAssetByType(Type type, string[] searchInFolders)
            => LoadAsset($"* t:{type.Name}", type, searchInFolders, false);

        public static T LoadAssetByTypeAndName<T>(string name, string[] searchInFolders)
            => (T)Convert.ChangeType(LoadAssetByTypeAndName(typeof(T), name, searchInFolders), typeof(T));

        public static UObject LoadAssetByTypeAndName(Type type, string name, string[] searchInFolders)
            => LoadAsset($"{name}", type, searchInFolders, true);

        public static T LoadPrefab<T>(string prefabName)
            => (T)Convert.ChangeType(LoadPrefab(typeof(T), prefabName), typeof(T));

        public static UObject LoadPrefab(Type type, string prefabName)
            => LoadAssetByTypeAndName(type, prefabName, new[] { Constants.FolderPaths.PREFABS });

        public static TSObject LoadSObject<TSObject>()
            => LoadAssetByType<TSObject>(new[] { Constants.FolderPaths.SCRIPTABLE_OBJECTS });
    }
}
