using UnityEngine;

namespace Core
{
    public static class GameObjectExtensions
    {
        public static bool HideComponentInInspector<TComponent>(this GameObject gameObject) 
            where TComponent : Component
        {
            var component = gameObject.GetComponent<TComponent>();
            if (component != null)
            {
                component.hideFlags = HideFlags.HideInInspector;
                return true;
            }
            return false;
        }
    }
}

