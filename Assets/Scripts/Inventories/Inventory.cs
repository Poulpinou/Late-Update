using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace LateUpdate {
    public class Inventory : MonoBehaviour
    {
        [SerializeField] float capacity;
        [SerializeField] List<ItemData> itemDatas = new List<ItemData>();

        public class InventoryUpdateEvent : UnityEvent { }
        public InventoryUpdateEvent onInventoryUpdate = new InventoryUpdateEvent();

        public float Encumbrance => itemDatas.Sum(i => i.Encumbrance);
        public float Capacity => capacity;
        public List<ItemData> ItemDatas => itemDatas;

        public bool Add(Item item, int amount = 1)
        {
            if(CanAdd(item, amount))
            {
                ItemData data = itemDatas.Where(i => i.Item == item).FirstOrDefault();
                if (data != null)
                    data.Amount += amount;
                else
                    itemDatas.Add(new ItemData(item, amount));
                UpdateInventory();
                return true;
            }
            else
            {
                MessageManager.Send(string.Format(
                    "Impossible to add {0} {1} to inventory, not enough capacity",
                    amount,
                    item.itemName
                ), LogType.Warning);
                return false;
            }
        }

        public void Remove(Item item, int amount = 1)
        {
            ItemData data = itemDatas.Where(i => i.Item == item).FirstOrDefault();
            if (data == null)
                MessageManager.Send(string.Format(
                    "No {0} found in inventory",
                    item.itemName
                ), LogType.Error);
            else
            {
                data.Amount -= amount;
                if(data.Amount <= 0)
                {
                    itemDatas.Remove(data);
                }
                UpdateInventory();
            }
        }

        public bool CanAdd(Item item, int amount = 1)
        {
            return Encumbrance + item.encumbrance * amount < capacity;
        }

        public float GetFillPercent()
        {
            return Encumbrance / capacity;
        }

        public void UpdateInventory()
        {
            onInventoryUpdate.Invoke();
        }

        private void Awake()
        {
            UpdateInventory();
        }
    }
}
