using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
{
    [CreateAssetMenu(fileName = "stringVariable", menuName = "JoHaToolkit/Variables/stringVariable")]
    public class StringVariable : SOVariableBase<string>
    {
        public StringVariable()
        {
            startValue = string.Empty;
            currentValue = string.Empty;
        }
    }

    [CustomPropertyDrawer(typeof(StringVariable))]
    public class StringVariableDrawer : VariableBaseDrawer<string>
    {
        
    }
}
