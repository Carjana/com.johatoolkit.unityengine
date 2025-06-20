using UnityEngine;

namespace JohaToolkit.UnityEngine.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent(out T component) ? component : gameObject.AddComponent<T>();
        }
        
        public static T GetOrAddComponent<T>(this Transform transform) where T : Component => GetOrAddComponent<T>(transform.gameObject);
        
        public static T GetOrAddComponent<T>(this Component component) where T : Component => component.gameObject.GetOrAddComponent<T>();
    }
}
