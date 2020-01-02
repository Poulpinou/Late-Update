using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    [Serializable]
    public class ItemData
    {
        [SerializeField] protected Item item;
        [SerializeField] protected int amount;
        Inventory inventory;

        public Item Item => item;
        public virtual int Amount { get => amount; set => amount = value; }
        public virtual float Encumbrance => item.encumbrance * amount;
        public virtual Inventory Inventory { get => inventory; internal set => inventory = value; }

        public ItemData(Inventory inventory, Item item, int amount = 1)
        {
            this.inventory = inventory;
            this.item = item;
            this.amount = amount;
        }

        public ItemData(Item item, int amount = 1) : this(null, item, amount) { }

        public override string ToString()
        {
            return string.Format("{0}(x{1})", item.itemName, amount);
        }

        public bool MoveTo(Inventory inventory)
        {
            if(inventory.CanAdd(this) && Inventory.Remove(this))
            {
                return inventory.Add(this);
            }
            return false;
        }

        public ItemData TakeAmount(int amount, bool affectInstance = false)
        {
            if (this.amount < amount)
                amount = this.amount;

            if(affectInstance)
                Amount -= amount;

            return new ItemData(inventory, item, amount);
        }

        public Pickable SpawnItem(Vector3 position)
        {
            GameObject go = GameObject.Instantiate(Item.model, GameManager.WorldObjectsRoot);
            Collider collider = go.GetComponent<Collider>();
            if (collider == null)
                collider = go.AddComponent<BoxCollider>();

            float offset = collider.bounds.size.y / 2;
            go.transform.position = position + new Vector3(0, offset, 0);

            InteractionAreaProvider areaProvider = go.AddComponent<InteractionAreaProvider>();
            areaProvider.FetchRadiusToCollider(collider);

            Pickable pickable = go.AddComponent<Pickable>();
            pickable.SetItemDatas(this);

            return pickable;
        }
    }
}
