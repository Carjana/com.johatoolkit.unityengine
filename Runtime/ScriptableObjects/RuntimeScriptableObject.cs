using System.Collections.Generic;
using UnityEngine;

namespace JohaToolkit.UnityEngine
{
    public abstract class RuntimeScriptableObject : ScriptableObject
    {
        private static readonly List<RuntimeScriptableObject> Instances = new();
        private void OnEnable() => Instances.Add(this);
        private void OnDisable() => Instances.Remove(this);
        protected abstract void OnReset();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ResetAll()
        {
            foreach (RuntimeScriptableObject runtimeScriptableObject in Instances)
            {
                runtimeScriptableObject.OnReset();
            }
        }
    }
}
