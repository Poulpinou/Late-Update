using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    /// <summary>
    /// This is the base class for <see cref="LateUpdate.Item"/> runtime datas
    /// </summary>
    [Serializable]
    public class ItemData
    {
        #region Serialized Fields
        [SerializeField] protected Item item;
        [SerializeField] protected int amount;
        #endregion

        #region Private Fields
        protected Inventory inventory;
        #endregion

        #region Public Properties
        /// <summary>
        /// The <see cref="ScriptableObject"/> <see cref="LateUpdate.Item"/> reference
        /// </summary>
        public Item Item => item;
        /// <summary>
        /// The amount of <see cref="LateUpdate.Item"/>
        /// </summary>
        public virtual int Amount { get => amount; set => amount = value; }
        /// <summary>
        /// The total encumbrance for those items
        /// </summary>
        public virtual float Encumbrance => item.encumbrance * amount;
        /// <summary>
        /// The <see cref="LateUpdate.Inventory"/> that owns this <see cref="ItemData"/>
        /// </summary>
        public virtual Inventory Inventory { get => inventory; internal set => inventory = value; }
        #endregion

        #region Constructors
        public ItemData(Inventory inventory, Item item, int amount = 1)
        {
            this.inventory = inventory;
            this.item = item;
            this.amount = amount;
        }

        public ItemData(Item item, int amount = 1) : this(null, item, amount) { }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("{0}(x{1})", item.itemName, amount);
        }

        /// <summary>
        /// Moves this <see cref="ItemData"/> from <see cref="Inventory"/> to <paramref name="inventory"/>
        /// </summary>
        /// <param name="inventory">The destination for those datas</param>
        /// <returns>True if success</returns>
        public bool MoveTo(Inventory inventory)
        {
            return inventory.Add(this);
        }

        /// <summary>
        /// Returns a new <see cref="ItemData"/> build from this and filled with <paramref name="amount"/>
        /// </summary>
        /// <param name="amount">The <see cref="Amount"/> ou the new item</param>
        /// <param name="affectInstance">If true, the original instance amount will be affected</param>
        /// <returns>A new <see cref="ItemData"/></returns>
        public ItemData TakeAmount(int amount, bool affectInstance = false)
        {
            if (this.amount < amount)
                amount = this.amount;

            if(affectInstance)
                Amount -= amount;

            return new ItemData(inventory, item, amount);
        }

        /// <summary>
        /// Spawn a <see cref="Pickable"/> item at <paramref name="position"/> filled with thoses datas
        /// </summary>
        /// <param name="position">The world position of the pickable</param>
        /// <returns>The instantiated item</returns>
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

            go.AddComponent<TooltipTarget>();

            return pickable;
        }

        public void Use()
        {
            try
            {
                Item.Use(Inventory.Owner as Actor);
                amount--;
                if (Inventory != null)
                    Inventory.UpdateInventory();
            }
            catch (Exception e)
            {
                MessageManager.Send(e.Message, LogType.Log);
            }
            
        }

        public void UseOn(WorldObject target)
        {
            try
            {
                Item.UseOn(Inventory.Owner as Actor, target);
                amount--;
                if (Inventory != null)
                    Inventory.UpdateInventory();
            }
            catch (Exception e)
            {
                MessageManager.Send(e.Message, LogType.Log);
            }
        }
        #endregion
    }
}
