using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class TypedInventory : Inventory
    {
        [Header("Limits")]
        [SerializeField] List<Item> allowedItems;

        public Item[] AllowedItems => allowedItems.ToArray();

        public override bool CanAdd(ItemData itemData)
        {
            if (!allowedItems.Contains(itemData.Item))
                return false;

            return base.CanAdd(itemData);
        }
    }
}
