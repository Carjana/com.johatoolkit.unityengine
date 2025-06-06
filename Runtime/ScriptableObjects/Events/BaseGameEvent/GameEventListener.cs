using UnityEngine;
using UnityEngine.Events;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent eventToListenTo;
        [SerializeField] private UnityEvent<object> response;

        private void OnEnable() => eventToListenTo?.Subscribe(Raise);

        private void OnDisable() => eventToListenTo?.Unsubscribe(Raise);

        private void Raise(object sender) => response?.Invoke(sender);
    }
}
