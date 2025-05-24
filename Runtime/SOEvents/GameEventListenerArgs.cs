using UnityEngine;
using UnityEngine.Events;

namespace JohaToolkit.UnityEngine.SOEvents
{
    public abstract class GameEventListener<T1> : MonoBehaviour
    {
        [SerializeField] private GameEvent<T1> eventToListenTo;
        [SerializeField] private UnityEvent<object, T1> response;

        private void OnEnable() => eventToListenTo?.Subscribe(Raise);

        private void OnDisable() => eventToListenTo?.Unsubscribe(Raise);

        private void Raise(object sender, T1 arg) => response?.Invoke(sender, arg);
    }
    
    public abstract class GameEventListener<T1, T2> : MonoBehaviour
    {
        [SerializeField] private GameEvent<T1, T2> eventToListenTo;
        [SerializeField] private UnityEvent<object, T1, T2> response;

        private void OnEnable() => eventToListenTo?.Subscribe(Raise);

        private void OnDisable() => eventToListenTo?.Unsubscribe(Raise);

        private void Raise(object sender, T1 arg1, T2 arg2) => response?.Invoke(sender, arg1, arg2);
    }
    
    public abstract class GameEventListener<T1, T2, T3> : MonoBehaviour
    {
        [SerializeField] private GameEvent<T1, T2, T3> eventToListenTo;
        [SerializeField] private UnityEvent<object, T1, T2, T3> response;

        private void OnEnable() => eventToListenTo?.Subscribe(Raise);

        private void OnDisable() => eventToListenTo?.Unsubscribe(Raise);

        private void Raise(object sender, T1 arg1, T2 arg2, T3 arg3) => response?.Invoke(sender, arg1, arg2, arg3);
    }
}
