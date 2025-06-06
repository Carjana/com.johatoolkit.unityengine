using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
{
    [CreateAssetMenu(fileName = "Vector3Variable", menuName = "JoHaToolkit/Variables/Vector3Variable")]
    public class Vector3Variable : SOVariableBase<Vector3>
    {
    }

    [CustomPropertyDrawer(typeof(Vector3Variable))]
    public class Vector3VariableDrawer : VariableBaseDrawer<Vector3>
    {
    }
}
