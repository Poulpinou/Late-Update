using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LateUpdate.Editors {
    [CustomEditor(typeof(DropContainer))]
    public class DropContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DropContainer dropContainer = target as DropContainer;
            dropContainer.ComputeAbsoluteDropChances();
            SerializedProperty dropProperty = serializedObject.FindProperty("dropChances");
            DropChance[] absoluteDropChances = dropContainer.AbsoluteDropChances;

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Drop Chances", EditorStyles.boldLabel);

            for (int i = 0; i < dropProperty.arraySize; i++)
            {
                DrawDropChanceLine(i, dropProperty.GetArrayElementAtIndex(i), absoluteDropChances[i]);
            }

            GUILayout.Label("Add new item : ", EditorStyles.miniBoldLabel);
            Object obj = null;
            obj = EditorGUILayout.ObjectField(obj, typeof(Item), false);

            if(obj != null) {
                int index = dropProperty.arraySize;
                dropProperty.InsertArrayElementAtIndex(index);
                SerializedProperty newElement = dropProperty.GetArrayElementAtIndex(index);
                newElement.FindPropertyRelative("item").objectReferenceValue = obj;
                newElement.FindPropertyRelative("weight").floatValue = 0.001f;
                serializedObject.ApplyModifiedProperties();
            }

            GUILayout.EndVertical();
        }

        void DrawDropChanceLine(int index, SerializedProperty dropChanceProperty, DropChance absoluteDropChance)
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            float weight = dropChanceProperty.FindPropertyRelative("weight").floatValue;

            if(absoluteDropChance.item == null)
            {
                GUILayout.Label("Error");
            }
            else
            {
                if (GUILayout.Button(absoluteDropChance.item.name, EditorStyles.label))
                {
                    Selection.activeObject = absoluteDropChance.item;
                }
                float newWeight = EditorGUILayout.Slider(weight, 0.001f, 1);
                GUILayout.Label((absoluteDropChance.weight).ToString("0.00%"));

                if(newWeight != weight)
                {
                    dropChanceProperty.FindPropertyRelative("weight").floatValue = newWeight;
                    serializedObject.ApplyModifiedProperties();
                }

                if (GUILayout.Button("-", EditorStyles.toolbarButton))
                {
                    serializedObject.FindProperty("dropChances").DeleteArrayElementAtIndex(index);
                    serializedObject.ApplyModifiedProperties();
                }
            }

            GUILayout.EndHorizontal();
        }
    }
}
