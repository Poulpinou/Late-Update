using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace LateUpdate {
    public class Inventory : MonoBehaviour, IInteractable
    {
        [SerializeField] float capacity;
        [SerializeField] List<ItemData> defaultItems = new List<ItemData>();

        List<ItemData> itemDatas;

        public class InventoryUpdateEvent : UnityEvent { }
        public InventoryUpdateEvent onInventoryUpdate = new InventoryUpdateEvent();

        public float Encumbrance => itemDatas.Sum(i => i.Encumbrance);
        public float Capacity => capacity;
        public List<ItemData> ItemDatas => itemDatas;

        public bool Add(Item item, int amount = 1)
        {
            return Add(new ItemData(item, amount));
        }

        public bool Add(ItemData itemData)
        {
            if (!CanAdd(itemData)) return false;

            ItemData localData = itemDatas.Where(i => i.Item == itemData.Item).FirstOrDefault();

            if (localData == null)
            {
                itemData.Inventory = this;
                itemDatas.Add(itemData);
            }
            else
            {
                localData.Amount += itemData.Amount;
            }

            UpdateInventory();
            return true;
        }

        public bool Remove(ItemData itemData)
        {
            ItemData localData = itemDatas.Where(i => i.Item == itemData.Item).FirstOrDefault();
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

        public void Drop(ItemData data)
        {
            if (Remove(data))
            {
                data.Inventory = null;
                data.SpawnItem(transform.position);
            }
        }

        public bool CanAdd(ItemData itemData)
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

        public float GetFillPercent()
        {
            return Encumbrance / capacity;
        }

        public void UpdateInventory()
        {
            onInventoryUpdate.Invoke();
        }

        void InitializeDatas()
        {
            itemDatas = new List<ItemData>();
            foreach(ItemData data in defaultItems)
            {
                data.Inventory = this;
                itemDatas.Add(data);
            }
        }

        private void Awake()
        {
            InitializeDatas();
            UpdateInventory();
        }

        public List<GameAction> GetPossibleActions(Actor actor)
        {
            return new List<GameAction> { new Trade_Action(actor, this) };
        }

        public class Trade_Action : GameAction
        {
            Inventory inventory;

            public override string Name => "Trade";

            public Trade_Action(Actor actor, Inventory target) : base(actor, target) {
                inventory = target;
            }

            public override void Execute()
            {
                UIManager.CreateFloatingPanel<InventoryPanel>(
                    Camera.main.WorldToScreenPoint(Target.gameObject.transform.position),
                    i => {
                        i.LinkInventory(inventory);
                        i.CloseCondition = IsTooFar;
                    }
                );
            }

            bool IsTooFar()
            {
                return Vector3.Distance(Actor.transform.position, Target.gameObject.transform.position) > 5;
            }
        }
    }
}
