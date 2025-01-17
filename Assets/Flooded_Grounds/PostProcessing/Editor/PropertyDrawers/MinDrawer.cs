using UnityEngine;
using UnityEditor;

public class MinAttribute : PropertyAttribute
{
    public readonly float min;

    public MinAttribute(float min)
    {
        this.min = min;
    }
}

[CustomPropertyDrawer(typeof(MinAttribute))]
public class MinDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinAttribute minAttribute = (MinAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            int value = EditorGUI.IntField(position, label, property.intValue);
            if (value < minAttribute.min)
            {
                value = (int)minAttribute.min;
            }
            property.intValue = value;
        }
        else if (property.propertyType == SerializedPropertyType.Float)
        {
            float value = EditorGUI.FloatField(position, label, property.floatValue);
            if (value < minAttribute.min)
            {
                value = minAttribute.min;
            }
            property.floatValue = value;
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use Min with float or int.");
        }
    }
}
