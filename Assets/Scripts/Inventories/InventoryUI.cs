using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyRPG {
    public class InventoryUI : MonoBehaviour
    {
        [Header("Relations")]
        [SerializeField] Button inventoryButton;
        [SerializeField] GameObject inventoryPanel;
        [SerializeField] Transform slotsParent;

        public Inventory Inventory { get; private set; }
        public InventorySlot[] Slots { get; private set; }

        public void SetInventory(Inventory inventory)
        {
            if (Inventory != null)
                Inventory.onInventoryChanged.RemoveListener(UpdateUI);

            Inventory = inventory;

            if (Inventory != null)
            {
                Inventory.onInventoryChanged.AddListener(UpdateUI);
                inventoryButton.interactable = true;
                UpdateUI();
            }
            else
            {
                inventoryButton.interactable = false;
                CloseInventory();
            }           
        }

        void UpdateUI()
        {
            Slots = slotsParent.GetComponentsInChildren<InventorySlot>();
            for (int i = 0; i < Slots.Length; i++)
            {
                if(Inventory != null &&  i < Inventory.Items.Count)
                {
                    Slots[i].SetItem(Inventory.Items[i]);
                }
                else
                {
                    Slots[i].ClearSlot();
                }
            }
        }

        public void OpenInventory()
        {
            if (Inventory == null) return;

            inventoryPanel.SetActive(true);
            UpdateUI();
        }

        public void CloseInventory()
        {
            inventoryPanel.SetActive(false);
        }

        public void SwitchInventory()
        {
            if (inventoryPanel.activeSelf)
                CloseInventory();
            else
                OpenInventory();
        }

        void OnCurrentControllerChanged(Controller controller)
        {
            SetInventory(controller == null? null : controller.GetComponent<Inventory>());
        }

        private void Update()
        {
            if (Input.GetButtonDown("Inventory"))
            {
                SwitchInventory();
            }
        }

        private void Start()
        {            
            inventoryButton.onClick.AddListener(SwitchInventory);
            OnCurrentControllerChanged(InputManager.CurrentController);
            InputManager.Active.onCurrentControllerChanged.AddListener(OnCurrentControllerChanged);
        }
    }
}
