using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ExposedSelector))]
public class ExposedSelectorPropertyDrawer : PropertyDrawer
{
    private static string[] _cachedFieldNames;
    private static Dictionary<string, int> _nameToIndex;
    private static long _lastUpdateFrame;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {


        SerializedProperty fieldNameProp = property.FindPropertyRelative("_fieldName");

        // Fetch the current list from Globals
        List<string> memberNames = Globals.GetMemberNames;

        // Get current index based on the stored field name
        int currentIndex = Mathf.Max(0, memberNames.IndexOf(fieldNameProp.stringValue));

        EditorGUI.BeginProperty(position, label, property);

        // Draw the popup
        int newIndex = EditorGUI.Popup(position, "", currentIndex, memberNames.ToArray());

        // If the selection changed, update the serialized value
        if (newIndex != currentIndex && newIndex >= 0 && newIndex < memberNames.Count)
        {
            fieldNameProp.stringValue = memberNames[newIndex];

            // This is crucial to force the change to be saved
            fieldNameProp.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }

   
}