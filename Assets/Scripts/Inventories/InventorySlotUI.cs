using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LateUpdate {
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Text nameText;
        [SerializeField] Text countText;
        [SerializeField] Image icon;

        ItemData datas;

        public void Configure(Inventory inventory, ItemData itemData)
        {
            datas = itemData;
            nameText.text = itemData.Item.itemName;
            countText.text = itemData.Amount.ToString();
            icon.sprite = itemData.Item.icon;
        }

        void Onclick()
        {
            datas.Item.Use();
        }

        private void Awake()
        {
            button.onClick.AddListener(Onclick);
        }
    }
}
