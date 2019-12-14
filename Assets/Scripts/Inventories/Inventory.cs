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
        [SerializeField] List<ContainerData> containers = new List<ContainerData>();

        public class InventoryUpdateEvent : UnityEvent { }
        public InventoryUpdateEvent onInventoryUpdate = new InventoryUpdateEvent();

        public ContainerData ActiveContainer { get; private set; } = null;
        public float Encumbrance => itemDatas.Sum(i => i.Encumbrance);
        public float Capacity => capacity;
        public List<ContainerData> Containers => containers;
        public List<ItemData> ActiveDataSet => ActiveContainer == null ? itemDatas : ActiveContainer.content;

        public void Transfer(ContainerData container1, ContainerData container2, Item item, int amount)
        {

        }

        public void SwitchActiveContainer(ContainerData containerData)
        {
            if (containerData == null || containers.Contains(containerData))
            {
                ActiveContainer = containerData;
                UpdateInventory();
            }
        }

        public bool Add(Item item, int amount = 1)
        {
            if(CanAdd(item, amount))
            {
                ItemData data = ActiveDataSet.Where(i => i.Item == item).FirstOrDefault();
                if (data != null)
                    data.Amount += amount;
                else
                    ActiveDataSet.Add(new ItemData(item, amount));
                UpdateInventory();
                return true;
            }
            else
            {
                MessageManager.Send(string.Format(
                    "Impossible to add {0} {1} to {2}, not enough capacity",
                    amount,
                    item.itemName,
                    ActiveContainer == null? name : ActiveContainer.Container.itemName
                ), LogType.Warning);
                return false;
            }
        }

        public void Remove(Item item, int amount = 1)
        {
            ItemData data = ActiveDataSet.Where(i => i.Item == item).FirstOrDefault();
            if (data == null)
                MessageManager.Send(string.Format(
                    "No {0} found in {1}",
                    item.itemName,
                    ActiveContainer == null ? name : ActiveContainer.Container.itemName
                ), LogType.Error);
            else
            {
                data.Amount -= amount;
                if(data.Amount <= 0)
                {
                    ActiveDataSet.Remove(data);
                }
                UpdateInventory();
            }
        }

        public bool CanAdd(Item item, int amount = 1)
        {
            if(ActiveContainer == null)
            {
                return Encumbrance + item.encumbrance * amount < capacity;
            }
            else
            {
                return ActiveContainer.Encumbrance + item.encumbrance * amount < ActiveContainer.Container.capacity;
            }
        }

        public float GetFillPercent()
        {
            return (Encumbrance + containers.Sum(c => c.Encumbrance)) / (capacity + containers.Sum(c => c.Container.capacity));
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
