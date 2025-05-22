using System;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SOVariables
{
    public class SOVariableBase<T> : RuntimeScriptableObject
    {
        [SerializeField] protected T startValue;
        [SerializeField] protected T currentValue;

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
