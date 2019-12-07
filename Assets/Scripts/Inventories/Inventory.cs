using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace LateUpdate {
    public class Inventory : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] List<Item> items = new List<Item>();
        [SerializeField] int space = 20;
        #endregion

        #region Events
        [Serializable] public class InventoryEvent : UnityEvent { }

        public InventoryEvent onInventoryChanged = new InventoryEvent();
        #endregion

        #region Public Properties
        public List<Item> Items => items;
        public int Space => space;
        #endregion

        #region Public Methods
        public bool Add(Item item)
        {
            if (!item.isDefaultItem)
            {
                if(items.Count >= space)
                {
                    Debug.Log("Not enough space");
                }
                items.Add(item);

                onInventoryChanged.Invoke();
                return true;
            }
            return false;
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            onInventoryChanged.Invoke();
        }

        public void Drop(Item item)
        {
            item.Spawn(transform.position);
            Remove(item);
        }
        #endregion
    }
}
