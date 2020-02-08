using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LateUpdate.Editors {
    [CustomEditor(typeof(WorldObject), true)]
    public class WorldObjectEditor : Editor
    {
        bool foldout = false;

        public override void OnInspectorGUI()
        {
            WorldObject worldObject = target as WorldObject;

            foldout = EditorGUILayout.Foldout(foldout, "WorldObject Infos");
            if (foldout)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                //GUILayout.Label("WorldObject Infos", EditorStyles.centeredGreyMiniLabel);

                GUILayout.Label("Collection Tags", EditorStyles.boldLabel);
                GUILayout.Label(string.Join(",", worldObject.GetCollectionTags()), EditorStyles.textField);
                GUILayout.Label("Components", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Amount : ", GUILayout.ExpandWidth(false));
                WorldObjectComponent[] components = worldObject.Components;
                GUILayout.Label(components.Length.ToString());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }

            DrawDefaultInspector();
        }
    }
}
