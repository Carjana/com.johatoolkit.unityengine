using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "JoHaToolkit/Variables/IntVariable")]
    public class IntVariable : SOVariableBase<int>
    {
    }

    [CustomPropertyDrawer(typeof(IntVariable))]
    public class IntVariableDrawer : VariableBaseDrawer<int>
    {
    }
}
