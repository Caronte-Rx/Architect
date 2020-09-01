using UnityEngine;

namespace Core
{
    public abstract class BaseSingletonSettings<TSettings> : SingletonSObject<TSettings>, IBaseSettings 
        where TSettings : ScriptableObject
    { }
}
