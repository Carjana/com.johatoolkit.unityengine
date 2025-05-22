using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JohaToolkit.UnityEngine.SOVariables
{
    public class VariableBaseDrawer<T> : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            ObjectField objectField = new(property.displayName)
            {
                objectType = typeof(SOVariableBase<T>),
            };
            
            objectField.BindProperty(property);
            
            Label valueLabel = new();
            valueLabel.style.paddingLeft = 20;
            
            container.Add(objectField);
            container.Add(valueLabel);
            
            objectField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is SOVariableBase<T> variable)
                {
                    SetContainer(valueLabel, variable);
                }
                else
                {
                    valueLabel.text = string.Empty;
                }
            });
            
            SOVariableBase<T> variable = property.objectReferenceValue as SOVariableBase<T>;
            if(variable != null)
            {
                SetContainer(valueLabel, variable);
            }
            
            return container;
        }

        private void SetContainer(Label valueLabel, SOVariableBase<T> variable)
        {
            valueLabel.text = GetValueText(variable.Value);
            
            variable.OnValueChanged += newValue =>
            {
                valueLabel.text = GetValueText(newValue);
            };
        }

        protected virtual string GetValueText(T value) => "Current Value: " + value.ToString();
    }
}
