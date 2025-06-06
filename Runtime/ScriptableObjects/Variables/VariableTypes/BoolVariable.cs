using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
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
