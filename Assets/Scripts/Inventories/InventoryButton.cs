using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LateUpdate {
    public class InventoryButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Image icon;
        [SerializeField] Slider fillBar;
        [SerializeField] Sprite nullContainerIcon;

        ContainerData containerData;
        Inventory inventory;

        public void Configure(Inventory inventory, ContainerData datas)
        {
            this.containerData = datas;
            this.inventory = inventory;

            icon.sprite = datas != null? datas.Container.icon : nullContainerIcon;
            UpdateFillBar();
        }

        void OnClick()
        {
            inventory.SwitchActiveContainer(containerData);
        }

        public void UpdateFillBar()
        {
            if (containerData != null)
                fillBar.value = containerData.Encumbrance / containerData.Container.capacity;
            else
                fillBar.value = inventory.Encumbrance / inventory.Capacity;
        }

        private void OnValidate()
        {
            if (button == null)
                button = GetComponent<Button>();
        }

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }
    }
}
