#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
public class RequireInterfaceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RequireInterfaceAttribute requiredAttribute = (RequireInterfaceAttribute)attribute;
        System.Type interfaceType = requiredAttribute.InterfaceType;

        // ��������� ���� ��� MonoBehavior
        EditorGUI.BeginProperty(position, label, property);

        UnityEngine.Object obj = EditorGUI.ObjectField(
            position,
            label,
            property.objectReferenceValue,
            typeof(MonoBehaviour),
            true
        );

        // ��������, ��������� �� ��������� ���������
        if (obj != null)
        {
            MonoBehaviour monoBehaviour = (MonoBehaviour)obj;
            if (!interfaceType.IsAssignableFrom(monoBehaviour.GetType()))
            {
                obj = null;
                Debug.LogError($"��������� ������ ������������� {interfaceType.Name}!");
            }
        }

        property.objectReferenceValue = obj;
        EditorGUI.EndProperty();
    }
}
#endif