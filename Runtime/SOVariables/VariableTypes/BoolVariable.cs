using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SOVariables.VariableTypes
{
    [CreateAssetMenu(fileName = "BoolVariable", menuName = "JoHaToolkit/Variables/BoolVariable")]
    public class BoolVariable : SOVariableBase<bool>
    {
    }

    [CustomPropertyDrawer(typeof(BoolVariable))]
    public class BoolVariableDrawer : VariableBaseDrawer<bool>
    {
    }
}
