using System;
using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.SOEvents
{
    public abstract class GameEvent<T1> : RuntimeScriptableObject
    {
        private readonly List<Action<object, T1>> _listeners = new();

        public void Subscribe(Action<object,T1> listener)
        {
            if (_listeners.Contains(listener))
                return;
            _listeners.Add(listener);
        }

        public void Unsubscribe(Action<object, T1> listener)
        {
            if (!_listeners.Contains(listener))
                return;
            _listeners.Remove(listener);
        }

        public void RaiseEvent(object sender, T1 arg)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke(sender, arg);
            }
        }

        protected override void OnReset()
        {
            _listeners.Clear();
        }
    }
    
    public abstract class GameEvent<T1, T2> : RuntimeScriptableObject
    {
        private readonly List<Action<object, T1, T2>> _listeners = new();

        public void Subscribe(Action<object, T1, T2> listener)
        {
            if (_listeners.Contains(listener))
                return;
            _listeners.Add(listener);
        }

        public void Unsubscribe(Action<object, T1, T2> listener)
        {
            if (!_listeners.Contains(listener))
                return;
            _listeners.Remove(listener);
        }

        public void RaiseEvent(object sender, T1 arg1, T2 arg2)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke(sender, arg1, arg2);
            }
        }

        protected override void OnReset()
        {
            _listeners.Clear();
        }
    }
    
    public abstract class GameEvent<T1, T2, T3> : RuntimeScriptableObject
    {
        private readonly List<Action<object, T1, T2, T3>> _listeners = new();

        public void Subscribe(Action<object, T1, T2, T3> listener)
        {
            if (_listeners.Contains(listener))
                return;
            _listeners.Add(listener);
        }

        public void Unsubscribe(Action<object, T1, T2, T3> listener)
        {
            if (!_listeners.Contains(listener))
                return;
            _listeners.Remove(listener);
        }

        public void RaiseEvent(object sender, T1 arg1, T2 arg2, T3 arg3)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke(sender, arg1, arg2, arg3);
            }
        }

        protected override void OnReset()
        {
            _listeners.Clear();
        }
    }
}
