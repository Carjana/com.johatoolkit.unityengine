using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SOVariables.VariableTypes
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "JoHaToolkit/Variables/FloatVariable")]
    public class FloatVariable : SOVariableBase<float>
    {
    }

    [CustomPropertyDrawer(typeof(FloatVariable))]
    public class FloatVariableDrawer : VariableBaseDrawer<float>
    {
    }
}
