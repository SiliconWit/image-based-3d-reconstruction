using UnityEngine;
using UnityEditor;

namespace ThaIntersect
{
    public class ReadOnlyAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false; // Disable editing
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;  // Enable editing again
        }
    }    
}
