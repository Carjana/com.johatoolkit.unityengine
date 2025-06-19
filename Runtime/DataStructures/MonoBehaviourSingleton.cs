using UnityEngine;
using UnityEngine.SceneManagement;

namespace JohaToolkit.UnityEngine.DataStructures
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;
        protected  bool IsPersistent { get; set; }

        protected void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"Singleton {nameof(T)} already exists, destroying GameObject {gameObject.name}.");
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this as T;
            }
            if (IsPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        protected void OnEnable()
        {
            if(IsPersistent)
                return;
            SceneManager.sceneUnloaded += OnSceneUnload;
        }
        
        protected void OnDisable()
        {
            if(IsPersistent)
                return;
            SceneManager.sceneUnloaded -= OnSceneUnload;
        }
        
        private void OnSceneUnload(Scene unloadedScene)
        {
            if (gameObject.scene != unloadedScene)
                return;
            
            Debug.Log($"Singleton {nameof(T)} is being destroyed because the scene {unloadedScene.name} was unloaded and {nameof(IsPersistent)} is false.");
            Instance = null;
            Destroy(gameObject);
        }
    }
}
