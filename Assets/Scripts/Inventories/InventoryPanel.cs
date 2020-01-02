using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

namespace LateUpdate
{
    public class InventoryPanel : UIPanel, IDropHandler
    {
        #region Enums
        public enum SortingOrder { none, AZ, encumbrance, amount }
        #endregion

        #region Serialized Fields
        [Header("Relations")]
        [SerializeField] protected Transform inventorySlotsParent;
        [SerializeField] protected Slider fillBar;

        [Header("Models")]
        [SerializeField] protected InventorySlotUI inventorySlotModel;
        #endregion

        #region Private Variables
        protected SortingOrder sortingOrder;
        #endregion

        #region Public Properties
        public Inventory Inventory { get; protected set; }
        #endregion

        #region Public Methods
        public virtual void LinkInventory(Inventory inventory)
        {
            if (Inventory != null)
            {
                Inventory.onInventoryUpdate.RemoveListener(OnInventoryUpdate);
            }

            Inventory = inventory;
            if(Inventory != null)
            {
                OnInventoryUpdate();
                Inventory.onInventoryUpdate.AddListener(OnInventoryUpdate);

                Actor actor = inventory.GetComponent<Actor>();
                if(actor != null)
                {
                    PanelName = actor.Infos.name + "'s Inventory";
                }
                else
                {
                    PanelName = "Inventory";
                }
            }
        }

        public void ChangeSortingOrder(SortingOrder order)
        {
            sortingOrder = order;
            RefreshInventorySlots();
        }

        public void ChangeSortingOrder(int order)
        {
            ChangeSortingOrder((SortingOrder)order);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.selectedObject == null) return;

            InventorySlotUI slotUI = eventData.selectedObject.GetComponent<InventorySlotUI>();
            if (slotUI == null) return;

            ItemData itemData = slotUI.ItemData;
            if (itemData.Inventory == Inventory) return;

            ActionManager.OpenAmountPopup(
                new AmountCallback<ItemData>(OnDrop, itemData),
                itemData.Amount,
                itemData.Amount
            );
        }
        #endregion

        #region Private Methods
        protected virtual void OnDrop(ItemData itemData, int amount)
        {
            itemData.TakeAmount(amount).MoveTo(Inventory);
        }

        protected virtual void RefreshInventorySlots()
        {
            InventorySlotUI[] slots = inventorySlotsParent.GetComponentsInChildren<InventorySlotUI>();
            List<ItemData> content = ApplySorting(Inventory.ItemDatas).ToList();

            for (int i = 0; i < content.Count || i < slots.Length; i++)
            {
                if (i < content.Count)
                {
                    InventorySlotUI slot;
                    if (i < slots.Length)
                    {
                        slot = slots[i];
                    }
                    else
                    {
                        slot = Instantiate(inventorySlotModel, inventorySlotsParent);
                        slot.onDragSlot.AddListener(OnSlotDrag);
                    }

                    slot.SetData(content[i]);
                }
                else
                {
                    Destroy(slots[i].gameObject);
                }
            }
        }

        protected virtual void OnSlotDrag(ItemData itemData, InventorySlotUI.SlotDragAction dragAction) { }

        protected virtual void RefreshFillBar()
        {
            if (fillBar == null) return;

            fillBar.value = Inventory.GetFillPercent();
        }

        protected virtual void OnInventoryUpdate()
        {
            if (Inventory == null) return;
            RefreshFillBar();
            RefreshInventorySlots();
        }

        protected virtual IEnumerable<ItemData> ApplySorting(IEnumerable<ItemData> datas)
        {
            switch (sortingOrder)
            {
                case SortingOrder.none:
                    return datas;
                case SortingOrder.AZ:
                    return datas.OrderBy(d => d.Item.itemName);
                case SortingOrder.encumbrance:
                    return datas.OrderBy(d => d.Encumbrance);
                case SortingOrder.amount:
                    return datas.OrderBy(d => d.Amount);
            }
            return datas;
        }
        #endregion
    }
}
