#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Отображаем список entries
        SerializedProperty entries = property.FindPropertyRelative("entries");
        EditorGUI.PropertyField(position, entries, label, true);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty entries = property.FindPropertyRelative("entries");
        return EditorGUI.GetPropertyHeight(entries);
    }
}
#endif