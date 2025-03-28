using System;
using System.Collections.Generic;
using UnityEngine;

namespace JohaToolkit.unity.GlobalEvents
{
    public static class GlobalEventsManager
    {
        private static readonly Dictionary<Type, List<Action>> Subscribers = new();

        public static void Subscribe<T>(Action action) where T : GlobalEventBase
        {
            Type type = typeof(T);
            if (!Subscribers.ContainsKey(type))
            {
                Subscribers.Add(type, new List<Action>());
            }

            Subscribers[type].Add(action);
        }

        public static void Unsuscribe<T>(Action action) where T : GlobalEventBase
        {
            Type type = typeof(T);
            if (!Subscribers.ContainsKey(type))
            {
                return;
            }

            Subscribers[type].Remove(action);
        }

        public static void Raise<T>() where T : GlobalEventBase
        {
            Type type = typeof(T);
            if (!Subscribers.ContainsKey(type))
            {
                Debug.LogWarning($"Event {type.Name} was raised, but doesnt exist!");
                return;
            }

            foreach (Action subscriber in Subscribers[type])
            {
                subscriber.Invoke();
            }
        }

        public abstract class GlobalEventBase
        {
            
        }
    }
}
