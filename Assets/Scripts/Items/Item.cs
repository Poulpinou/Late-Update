using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace LateUpdate {
    [CreateAssetMenu(menuName = "TinyRPG/Items/Item", fileName = "New Item", order = 0)]
    public class Item : ScriptableObject
    {
        #region Public Fields
        [Header("General infos")]
        public string itemName = null;
        public Sprite icon = null;
        public GameObject model;

        [Header("Inventory")]
        public float encumbrance = 0.1f;
        #endregion

        #region Public Methods
        public virtual void Use(Actor actor)
        {
            throw new Exception(string.Format("{0} is using {1} : Nothing happens", actor.Infos.name, itemName));
        }

        public virtual void UseOn(Actor actor, WorldObject target)
        {
            throw new Exception(string.Format("{0} can't use {1} on {2}", actor.Infos.name, itemName, target.name));
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(itemName))
                itemName = name;
        }

        [ContextMenu("Update Icon")]
        void SetIcon()
        {
            var ty = typeof(EditorGUIUtility);
            var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            mi.Invoke(null, new object[] { this, icon.texture });
        }
#endif
        #endregion
    }
}
