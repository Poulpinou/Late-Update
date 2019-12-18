using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace LateUpdate {
    public class InventorySlotUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Enum
        public enum SlotDragAction { startDrag, drag, endDrag }
        #endregion

        #region Serialized Fields
        [Header("Required")]
        [SerializeField] Button button;
        [SerializeField] Text countText;
        [SerializeField] Image icon;

        [Header("Optional")]
        [SerializeField] Text nameText;
        #endregion

        #region Private Fields
        ItemData datas;
        Image tempDragger;
        #endregion

        #region Events
        public class DragSlotEvent : UnityEvent<ItemData, SlotDragAction> { }
        public DragSlotEvent onDragSlot = new DragSlotEvent();
        #endregion

        #region Public Methods
        public void SetData(ItemData itemData)
        {
            datas = itemData;

            if(nameText != null)
                nameText.text = itemData.Item.itemName;

            countText.text = itemData.Amount.ToString();
            icon.sprite = itemData.Item.icon;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (tempDragger != null)
            {
                tempDragger.transform.position = Input.mousePosition;
                onDragSlot.Invoke(datas, SlotDragAction.drag);
            }    
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(tempDragger != null)
            {
                Destroy(tempDragger);
                onDragSlot.Invoke(datas, SlotDragAction.endDrag);
            }

            icon.enabled = true;
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            tempDragger = Instantiate(icon, GameManager.UIRoot);
            tempDragger.rectTransform.sizeDelta = icon.rectTransform.sizeDelta;
            icon.enabled = false;

            onDragSlot.Invoke(datas, SlotDragAction.startDrag);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.Show(datas.Item.itemName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.Hide();
        }
        #endregion

        #region Private Methods
        void Onclick()
        {
            datas.Item.Use();
        }
        #endregion

        #region Runtime Methods
        private void Awake()
        {
            button.onClick.AddListener(Onclick);
        }
        #endregion
    }
}
