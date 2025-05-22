using UnityEditor;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SOVariables.VariableTypes
{
    public enum MyEnum
    {
        Option1,
        Option2,
        Option3
    }
    
    [CreateAssetMenu(fileName = "ExampleEnumVariable", menuName = "JoHaToolkit/Variables/ExampleEnumVariable")]
    public class ExampleEnumVariable : SOVariableBase<MyEnum>
    {
    }

    [CustomPropertyDrawer(typeof(ExampleEnumVariable))]
    public class ExampleEnumVariableDrawer : VariableBaseDrawer<MyEnum>
    {
    }
}
