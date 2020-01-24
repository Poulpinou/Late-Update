using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using LateUpdate.Actions;

namespace LateUpdate {
    /// <summary>
    /// Attach this to a <see cref="GameObject"/> to provide it an inventory
    /// </summary>
    public class Inventory : MonoBehaviour, IInteractable
    {
        #region Serialized Fields
        [Header("Capacity")]
        [SerializeField] bool modifiedByScript = true;
        [SerializeField][ConditionalField("modifiedByScript", true)] float capacity;
        [Header("Content")]
        [Tooltip("Items that exists by default in this inventory")]
        [SerializeField] List<ItemData> defaultItems = new List<ItemData>();
        #endregion

        #region Private Fields
        List<ItemData> itemDatas;
        #endregion

        #region Events
        public class InventoryUpdateEvent : UnityEvent { }
        /// <summary>
        /// Is called when inventory content changed
        /// </summary>
        public InventoryUpdateEvent onInventoryUpdate = new InventoryUpdateEvent();
        #endregion

        #region Public Properties
        /// <summary>
        /// The total encumbrance of the inventory
        /// </summary>
        public float Encumbrance => itemDatas.Sum(i => i.Encumbrance);
        /// <summary>
        /// The max <see cref="Encumbrance"/> of the inventory
        /// </summary>
        public float Capacity
        {
            get => capacity;
            set {
                if (modifiedByScript)
                    capacity = value;
            }
        }
        public float AvailableSpace => Capacity - Encumbrance;
        /// <summary>
        /// The datas of every items of the inventory
        /// </summary>
        public List<ItemData> ItemDatas => itemDatas;
        #endregion

        #region Public Methods
        public ItemData GetDataFromItem(Item item)
        {
            return itemDatas.Where(i => i.Item == item).FirstOrDefault();
        }

        public bool Contains(Item item)
        {
            return itemDatas.Any(i => i.Item == item);
        }

        public void AddMax(ref ItemData itemData)
        {
            ItemData datasToAdd = itemData.TakeAmount(MaxAddableAmount(itemData.Item));
            Add(datasToAdd);
        }

        public int MaxAddableAmount(Item item)
        {
            return Mathf.FloorToInt(AvailableSpace / item.encumbrance);
        }

        /// <summary>
        /// Adds <paramref name="amount"/> x <paramref name="item"/> to the <see cref="Inventory"/>
        /// </summary>
        /// <param name="item">The type of item to add</param>
        /// <param name="amount">The amount of item to add</param>
        /// <returns>True if success</returns>
        public bool Add(Item item, int amount = 1)
        {
            return Add(new ItemData(item, amount));
        }

        /// <summary>
        /// Adds <paramref name="itemData"/> to <see cref="Inventory"/>
        /// </summary>
        /// <param name="itemData">Datas to add</param>
        /// <returns>True if success</returns>
        public virtual bool Add(ItemData itemData)
        {
            if (!CanAdd(itemData)) return false;
            if (itemData.Amount <= 0) return true;        

            if(itemData.Inventory == null || itemData.Inventory.Remove(itemData))
            {
                ItemData localData = GetDataFromItem(itemData.Item);
                if (localData == null)
                {
                    itemData.Inventory = this;
                    itemDatas.Add(itemData);
                }
                else
                {
                    localData.Amount += itemData.Amount;
                }
            }

            UpdateInventory();
            return true;
        }

        /// <summary>
        /// Removes <paramref name="itemData"/> from <see cref="Inventory"/>
        /// </summary>
        /// <param name="itemData">Datas to remove</param>
        /// <returns>True if success</returns>
        public virtual bool Remove(ItemData itemData)
        {
            ItemData localData = GetDataFromItem(itemData.Item);
            if (localData == null)
            {
                MessageManager.Send(string.Format(
                    "No {0} found in inventory",
                    itemData.Item.itemName
                ), LogType.Error);
                return false;
            }
            else
            {
                localData.Amount -= itemData.Amount;
                if (localData.Amount <= 0)
                {
                    itemDatas.Remove(localData);
                }
                UpdateInventory();
                return true;
            }
        }

        /// <summary>
        /// Drops <paramref name="data"/> from <see cref="Inventory"/> and spawns a <see cref="Pickable"/> on the floor
        /// </summary>
        /// <param name="data">Datas to drop</param>
        public virtual void Drop(ItemData data)
        {
            if (Remove(data))
            {
                data.Inventory = null;
                data.SpawnItem(transform.position);
            }
        }

        /// <summary>
        /// Returns true if <paramref name="itemData"/> can be add to inventory
        /// </summary>
        /// <param name="itemData">The datas to add</param>
        /// <returns>True if can add</returns>
        public virtual bool CanAdd(ItemData itemData)
        {
            if (itemData.Inventory == this)
            {
                Debug.LogError(
                    string.Format(
                        "Tried to add {0} to its own inventory ({1})",
                        itemData,
                        name
                    )
                );
                return false;
            }

            if (Encumbrance + itemData.Encumbrance > capacity)
            {
                Debug.LogError(
                    string.Format(
                        "Impossible to add {0} to inventory, not enough capacity",
                        itemData
                    )
                );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the <see cref="Encumbrance"/> on <see cref="Capacity"/> ratio
        /// </summary>
        /// <returns></returns>
        public float GetFillPercent()
        {
            return Encumbrance / capacity;
        }

        /// <summary>
        /// Call this to tell that the <see cref="Inventory"/> has been updated
        /// </summary>
        public void UpdateInventory()
        {
            onInventoryUpdate.Invoke();
        }

        public List<GameAction> GetPossibleActions(Actor actor)
        {
            return new List<GameAction> { new Trade_Action(actor, this) };
        }
        #endregion

        #region Private Methods
        void InitializeDatas()
        {
            itemDatas = new List<ItemData>();
            foreach(ItemData data in defaultItems)
            {
                data.Inventory = this;
                itemDatas.Add(data);
            }
        }
        #endregion

        #region Runtime Methods
        private void Awake()
        {
            InitializeDatas();
            UpdateInventory();
        }
        #endregion
    }
}
