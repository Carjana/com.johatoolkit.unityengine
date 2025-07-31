using UnityEditor;
using UnityEngine;

namespace Packages.com.johatoolkit.unityengine.Editor
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class CustomScriptableObjectPropertyDrawer : PropertyDrawer
    {
        // Cached scriptable object editor
        private UnityEditor.Editor editor = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw label
            EditorGUI.PropertyField(position, property, label, true);
 
            // Draw foldout arrow
            if (property.objectReferenceValue != null)
            {
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            }

            if (!property.objectReferenceValue)
                return;
            
            // Draw foldout properties
            if (property.isExpanded)
            {
                // Make child fields be indented
                EditorGUI.indentLevel++;
     
                // Draw object properties
                if (!editor)
                    UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
                
                if (!editor)
                    return;
                
                editor.OnInspectorGUI();
     
                // Set indent back to what it was
                EditorGUI.indentLevel--;
            }
        }
    }
}
