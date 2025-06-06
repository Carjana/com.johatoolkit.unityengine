using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
{
    [CreateAssetMenu(fileName = "Vector2Variable", menuName = "JoHaToolkit/Variables/Vector2Variable")]
    public class Vector2Variable : SOVariableBase<Vector2>
    {
    }

    [CustomPropertyDrawer(typeof(Vector2Variable))]
    public class Vector2VariableDrawer : VariableBaseDrawer<Vector2>
    {
        
    }
}
