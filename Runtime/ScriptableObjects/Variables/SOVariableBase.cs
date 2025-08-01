using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
{
    public class SOVariableBase<T> : RuntimeScriptableObject
    {
        [SerializeField] protected T startValue;
        [SerializeField] protected T currentValue;

        [Button]
        private void UpdateValue()
        {
            OnValueChanged?.Invoke(currentValue);
        }
        
        public event Action<T> OnValueChanged;
        public T Value
        {
            get => currentValue;
            set
            {
                if (currentValue.Equals(value))
                    return;
                currentValue = value;
                OnValueChanged?.Invoke(currentValue);
            }
        }

        protected override void OnReset()
        {
            currentValue = startValue;
        }
    }
}
