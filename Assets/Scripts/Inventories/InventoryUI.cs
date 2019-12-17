using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace LateUpdate {
    public class InventoryUI : MonoBehaviour
    {
        #region Enums
        public enum SortingOrder {none, AZ, encumbrance, amount }
        #endregion

        #region Serialized Fields
        [Header("Relations")]
        [SerializeField] TranslationSwitcher switchablePanel;
        [SerializeField] Transform inventorySlotsParent;
        [SerializeField] Slider fillBar;
        [SerializeField] DropHandler trashHandler;

        [Header("Models")]
        [SerializeField] InventorySlotUI inventorySlotModel;
        #endregion

        #region Private Variables
        SortingOrder sortingOrder;
        ItemData tempDatas;
        #endregion

        #region Public Properties
        public Inventory Inventory { get; private set; }
        #endregion

        #region Public Methods
        public void SetInventory(Inventory inventory)
        {
            if(Inventory != null)
            {
                Inventory.onInventoryUpdate.RemoveListener(OnInventoryUpdate);
            }

            Inventory = inventory;

            if (Inventory == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            RefreshFillBar();
            RefreshInventorySlots();

            inventory.onInventoryUpdate.AddListener(OnInventoryUpdate);
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
        #endregion

        #region Private Methods
        void RefreshInventorySlots()
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

        void OnSlotDrag(ItemData itemData, InventorySlotUI.SlotDragAction dragAction)
        {
            switch (dragAction)
            {
                case InventorySlotUI.SlotDragAction.startDrag:
                    trashHandler.gameObject.SetActive(true);
                    tempDatas = itemData;
                    trashHandler.onDrop.AddListener(delegate {
                        ActionManager.OpenAmountPopup(PerformDrop, tempDatas.Amount, tempDatas.Amount, 0);
                    });
                    break;
                case InventorySlotUI.SlotDragAction.drag:
                    break;
                case InventorySlotUI.SlotDragAction.endDrag:
                    trashHandler.gameObject.SetActive(false);
                    trashHandler.onDrop.RemoveAllListeners();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Drops <see cref="tempDatas"/> (is called by <see cref="AmountPopup"/>)
        /// </summary>
        /// <param name="amount">The amount to drop</param>
        void PerformDrop(int amount)
        {
            Inventory.Remove(tempDatas.Item, amount);
            tempDatas = null;
        }

        void RefreshFillBar()
        {
            fillBar.value = Inventory.GetFillPercent();
        }

        void OnCurrentControllerChanged(Controller controller)
        {
            SetInventory(controller == null ? null : controller.GetComponent<Inventory>());
        }

        void OnInventoryUpdate()
        {
            RefreshFillBar();
            RefreshInventorySlots();
        }

        IEnumerable<ItemData> ApplySorting(IEnumerable<ItemData> datas)
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

        #region Runtime Methods
        private void Start()
        {
            OnCurrentControllerChanged(InputManager.CurrentController);
            InputManager.Active.onCurrentControllerChanged.AddListener(OnCurrentControllerChanged);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Inventory"))
                switchablePanel.Switch();
        }
        #endregion
    }
}
