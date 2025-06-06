using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
{
    [CreateAssetMenu(fileName = "ColorVariable", menuName = "JoHaToolkit/Variables/ColorVariable")]
    public class ColorVariable : SOVariableBase<Color>
    {
    }

    [CustomPropertyDrawer(typeof(ColorVariable))]
    public class ColorVariableDrawer : VariableBaseDrawer<Color>
    {
    }
}
