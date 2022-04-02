#if (UNITY_EDITOR) 
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AttackSpecifications))]
public class AttackSpecificationsPropertyDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // The 6 comes from extra spacing between the fields (2px each)
        return EditorGUIUtility.singleLineHeight * 4 + 6;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, 16), label);
        // EditorGUI.LabelField(position, label);
        EditorGUI.indentLevel++;

        var attackDirectionRect = new Rect(position.x, position.y + 18, position.width, 16);
        EditorGUI.PropertyField(attackDirectionRect, property.FindPropertyRelative("attackDirection"));

        var knockbackRect = new Rect(position.x, position.y + 36, position.width, 16);
        EditorGUI.PropertyField(knockbackRect, property.FindPropertyRelative("knockback"));

        var damageRect = new Rect(position.x, position.y + 54, position.width, 16);
        EditorGUI.PropertyField(damageRect, property.FindPropertyRelative("damage"));

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();

    }

}
#endif