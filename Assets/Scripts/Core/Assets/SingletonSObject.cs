using UnityEngine;

namespace Core
{
    public abstract class SingletonSObject<TSObject> : ScriptableObject where TSObject : ScriptableObject
    {
        public static TSObject SObject
        {
            get
            {
                if (_sobject == null)
                    _sobject = AssetUtility.LoadSObject<TSObject>();
                return _sobject;
            }
        }

        private static TSObject _sobject = null;
    }
}
