using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Variables
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
}
