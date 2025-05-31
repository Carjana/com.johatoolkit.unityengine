using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JohaToolkit.UnityEngine.DataStructures
{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;
        protected abstract bool IsPersistent { get; }
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"Singleton {nameof(T)} already exists, destroying GameObject {gameObject.name}.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this as T;
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneUnload;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnload;
        }
        
        private void OnSceneUnload(Scene unloadedScene)
        {
            if (gameObject.scene != unloadedScene || IsPersistent)
                return;
            
            Debug.Log($"Singleton {nameof(T)} is being destroyed because the scene {unloadedScene.name} was unloaded and {nameof(IsPersistent)} is false.");
            Instance = null;
            Destroy(gameObject);
        }
    }
}
