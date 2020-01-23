using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LateUpdate.Editors {
    [CustomEditor(typeof(WorldObjectManager))]
    public class WorldObjectManagerEditor : Editor
    {
        Dictionary<string, bool> states = new Dictionary<string, bool>();

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!Application.isPlaying) return;

            WorldObjectManager manager = target as WorldObjectManager;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Collections", EditorStyles.boldLabel);

            foreach (KeyValuePair<string, List<WorldObject>> collection in manager.Collections)
            {
                if (!states.ContainsKey(collection.Key))
                    states.Add(collection.Key, false);

                states[collection.Key] = EditorGUILayout.Foldout(states[collection.Key], string.Format("{0}({1})", collection.Key, collection.Value.Count));
                if (states[collection.Key])
                {
                    int i = 1;
                    foreach(WorldObject worldObject in collection.Value)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(i + "-" + worldObject.name, EditorStyles.label);
                        if (GUILayout.Button("See", EditorStyles.miniButton))
                        {
                            Selection.activeObject = worldObject.gameObject;
                        }
                        if (GUILayout.Button("Kill", EditorStyles.miniButton))
                        {
                            Destroy(worldObject.gameObject);
                        }
                        EditorGUILayout.EndHorizontal();
                        i++;
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }
    }
}
