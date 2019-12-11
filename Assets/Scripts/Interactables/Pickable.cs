using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LateUpdate {
    public class Pickable : Interactable
    {
        #region Serialized Fields
        [Header("Pickable")]
        [SerializeField] Item item;
        #endregion

        #region Public Properties
        public Item Item => item;
        #endregion

        #region Private Methods
        internal void SetItem(Item item)
        {
            this.item = item;
            name = "Pickable_" + item.itemName;
            item.pickable = this;
        }

        protected override void Interact()
        {
            PickUp();
        }

        protected virtual void PickUp()
        {
            Inventory inventory = actor.GetComponent<Inventory>();
            if (inventory == null)
                throw new Exception(string.Format("{0} can't pickup {1} because it has no Inventory attached to it!", actor.name, item.itemName));

            if (inventory.Add(item))
            {          
                Destroy(gameObject);
            }
        }
        #endregion

#if UNITY_EDITOR
        #region Editor Methods
        [ContextMenu("Link To Item")]
        void LinkToItem()
        {
            if(PrefabUtility.GetCorrespondingObjectFromSource(gameObject) != null)
            {
                throw new Exception("This method should be called on prefab!");
            }

            item.pickable = this;
        }
        #endregion
#endif
    }
}
