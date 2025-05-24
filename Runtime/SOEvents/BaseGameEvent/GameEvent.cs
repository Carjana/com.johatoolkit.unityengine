using System;
using System.Collections.Generic;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SOEvents
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "JoHaToolkit/Events/GameEvent")]
    public class GameEvent : RuntimeScriptableObject
    {
        private readonly List<Action<object>> _listeners = new();

        public void Subscribe(Action<object> callBack)
        {
            if (_listeners.Contains(callBack))
                return;
            _listeners.Add(callBack);
        }

        public void Unsubscribe(Action<object> listener)
        {
            if (!_listeners.Contains(listener))
                return;
            _listeners.Remove(listener);
        }

        public void RaiseEvent(object sender)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke(sender);
            }
        }

        protected override void OnReset()
        {
            _listeners.Clear();
        }
    }
}
