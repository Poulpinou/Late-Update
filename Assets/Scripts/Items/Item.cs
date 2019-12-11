using UnityEngine;

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

        [Header("Relations")]
        public Pickable pickable;

        //K zone => delete if not used
        [Header("Inventory K")]
        [Tooltip("The size of the item in Inventory Grid")]
        public Vector2Int inventorySize = Vector2Int.one;
        [Tooltip("The max amount of items for one stack, 1 == not stackable")]
        public int stackLimit = 1;

        [Header("Inventory")]
        public float encumbrance = 0.1f;
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

        /*public virtual Inventory GetActiveInventory()
        {
            return InputManager.CurrentController.GetComponent<Inventory>();
        }*/

        public virtual void Use()
        {
            Debug.Log("Using " + itemName);
        }

        public virtual void RemoveFromInventory()
        {
            //GetActiveInventory().Remove(this);
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(itemName))
                itemName = name;

            if (inventorySize.x < 1) inventorySize.x = 1;
            if (inventorySize.y < 1) inventorySize.y = 1;
            if (stackLimit < 1) stackLimit = 1;
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
