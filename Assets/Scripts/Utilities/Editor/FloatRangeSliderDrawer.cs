using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
public class FloatRangeSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        int originalIndentLevel = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position,
            GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel = 0;
        SerializedProperty minProp = property.FindPropertyRelative("min");
        SerializedProperty maxProp = property.FindPropertyRelative("max");
        float minValue = minProp.floatValue;
        float maxValue = maxProp.floatValue;
        float fieldWidth = position.width / 4f - 4f;
        float sliderWidth = position.width / 2f;
        position.width = fieldWidth;
        minValue = EditorGUI.FloatField(position, minValue);
        position.x += fieldWidth + 4f;
        position.width = sliderWidth;
        FloatRangeSliderAttribute limit = attribute as FloatRangeSliderAttribute;
        EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, limit.Min, limit.Max);
        position.x += sliderWidth + 4f;
        position.width = fieldWidth;
        maxValue = EditorGUI.FloatField(position, maxValue);
        if(minValue < limit.Min) {
            minValue = limit.Min;
        }
        if(maxValue < minValue) {
            maxValue = minValue;
        }
        else if(maxValue > limit.Max) {
            maxValue = limit.Max;
        }
        minProp.floatValue = minValue;
        maxProp.floatValue = maxValue;

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = originalIndentLevel;
    }
}
