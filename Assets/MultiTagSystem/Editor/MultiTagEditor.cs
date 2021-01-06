using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MultiTagSystem
{
    [CustomEditor(typeof(MultiTagComponent))]
    public class MultiTagEditor : Editor
    {
        private string[] UnityTags;
        private SerializedProperty TagsProp;
        private ReorderableList List;

        private void OnEnable()
        {
            UnityTags = InternalEditorUtility.tags;
            TagsProp = serializedObject.FindProperty("_Tags");
            List = new ReorderableList(serializedObject, TagsProp, true, true, true, true);
            List.drawHeaderCallback += DrawHeader;
            List.drawElementCallback += DrawElement;
            List.onAddDropdownCallback += OnAddDropdown;
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, new GUIContent("Tags"), EditorStyles.boldLabel);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = List.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.LabelField(rect, element.stringValue);
        }

        private void OnAddDropdown(Rect buttonRect, ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < UnityTags.Length; i++)
            {
                var label = new GUIContent(UnityTags[i]);

                // Don't allow duplicate tags to be added.
                if (PropertyContainsString(TagsProp, UnityTags[i]))
                    menu.AddDisabledItem(label);
                else
                    menu.AddItem(label, false, OnAddClickHandler, UnityTags[i]);
            }

            menu.ShowAsContext();
        }

        private bool PropertyContainsString(SerializedProperty property, string value)
        {
            if (property.isArray)
            {
                for (int i = 0; i < property.arraySize; i++)
                {
                    if (property.GetArrayElementAtIndex(i).stringValue == value)
                        return true;
                }
            }
            else
                return property.stringValue == value;

            return false;
        }

        private void OnAddClickHandler(object tag)
        {
            int index = List.serializedProperty.arraySize;
            List.serializedProperty.arraySize++;
            List.index = index;

            var element = List.serializedProperty.GetArrayElementAtIndex(index);
            element.stringValue = (string)tag;
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(6);
            serializedObject.Update();
            List.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(3);
        }

    }
}