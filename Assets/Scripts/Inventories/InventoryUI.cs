using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace LateUpdate {
    public class InventoryUI : MonoBehaviour
    {
        public enum SortingOrder {none, AZ, encumbrance, amount }

        [Header("Relations")]
        [SerializeField] TranslationSwitcher switchablePanel; 
        [SerializeField] Transform inventoryButtonsParent;
        [SerializeField] Transform inventorySlotsParent;
        [SerializeField] Slider fillBar;

        [Header("Models")]
        [SerializeField] InventoryButton inventoryButtonModel;
        [SerializeField] InventorySlotUI inventorySlotModel;

        SortingOrder sortingOrder;

        public Inventory Inventory { get; private set; }

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

            RefreshInventoryButtons();
            RefreshFillBar();
            if (switchablePanel.IsOpen)
            {
                RefreshInventorySlots();
            }

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

        void RefreshInventoryButtons()
        {
            InventoryButton[] buttons = inventoryButtonsParent.GetComponentsInChildren<InventoryButton>();
            int nbcontainers = Inventory.Containers.Count + 1;

            for (int i = 0; i < nbcontainers || i < buttons.Length ; i++)
            {
                ContainerData containerData = i == 0 ? null : Inventory.Containers[i - 1];

                if(i < nbcontainers)
                {
                    if (i < buttons.Length)
                    {
                        buttons[i].Configure(Inventory, containerData);
                    }
                    else
                    {
                        InventoryButton button = Instantiate(inventoryButtonModel, inventoryButtonsParent);
                        button.Configure(Inventory, containerData);
                    }
                }
                else
                {
                    Destroy(buttons[i].gameObject);
                }
            }
        }

        void RefreshInventorySlots()
        {
            InventorySlotUI[] slots = inventorySlotsParent.GetComponentsInChildren<InventorySlotUI>();
            List<ItemData> content = ApplySorting(Inventory.ActiveDataSet).ToList();

            for (int i = 0; i < content.Count || i < slots.Length; i++)
            {
                if (i < content.Count)
                {
                    if (i < slots.Length)
                    {
                        slots[i].Configure(Inventory, content[i]);
                    }
                    else
                    {
                        InventorySlotUI slot = Instantiate(inventorySlotModel, inventorySlotsParent);
                        slot.Configure(Inventory, content[i]);
                    }
                }
                else
                {
                    Destroy(slots[i].gameObject);
                }
            }
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
            InventoryButton[] buttons = inventoryButtonsParent.GetComponentsInChildren<InventoryButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].UpdateFillBar();
            }
            RefreshFillBar();

            if (switchablePanel.IsOpen)
            {
                RefreshInventorySlots();
            }
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
    }
}
