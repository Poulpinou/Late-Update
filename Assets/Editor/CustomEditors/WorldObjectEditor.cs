using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LateUpdate.Editors {
    [CustomEditor(typeof(WorldObject), true)]
    public class WorldObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            WorldObject worldObject = target as WorldObject;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Collection Tags", EditorStyles.miniBoldLabel);
            GUILayout.Label(string.Join(",", worldObject.GetCollectionTags()), EditorStyles.textField);
            EditorGUILayout.EndVertical();

            DrawDefaultInspector();
        }
    }
}
