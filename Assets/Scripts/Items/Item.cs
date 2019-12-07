using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace TinyRPG {
    [CreateAssetMenu(menuName = "TinyRPG/Items/Item", fileName = "New Item", order = 0)]
    public class Item : ScriptableObject
    {
        #region Public Fields
        [Header("General infos")]
        public string itemName = null;
        public Sprite icon = null;       
        public bool isDefaultItem = false;

        [Header("Relations")]
        public Pickable pickable;
        #endregion

        #region Public Methods
        public Pickable Spawn(Vector3 position, Transform parent = null)
        {
            if (pickable == null)
                throw new System.Exception(string.Format("Impossible to spawn {0}, it has no Pickable defined", name));

            Pickable inst = Instantiate(pickable, parent);
            inst.transform.position += position;

            return inst;
        }
        public virtual Inventory GetActiveInventory()
        {
            return InputManager.CurrentController.GetComponent<Inventory>();
        }

        public virtual void Use()
        {
            Debug.Log("Using " + itemName);
        }

        public virtual void RemoveFromInventory()
        {
            GetActiveInventory().Remove(this);
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
