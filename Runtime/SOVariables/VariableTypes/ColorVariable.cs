using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JohaToolkit.UnityEngine.SOVariables.VariableTypes
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
