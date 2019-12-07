using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TinyRPG {
    public class InventorySlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        #region Serialized Fields
        [SerializeField] Image icon;
        #endregion

        #region Private Fields
        Item item;
        Image tempImage;
        #endregion

        #region Public Methods
        public InventoryUI GetInventoryUI()
        {
            return GetComponentInParent<InventoryUI>();
        }

        public void SetItem(Item newItem)
        {
            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;
        }

        public void ClearSlot()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;
        }

        public void UseItem()
        {
            if(item != null)
                item.Use();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(tempImage != null)
                tempImage.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (tempImage == null) return;

            icon.enabled = true;
            Destroy(tempImage.gameObject);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GetInventoryUI().Inventory.Drop(item);
            }

            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item == null) return;

            EventSystem.current.SetSelectedGameObject(gameObject);
            tempImage = Instantiate(icon, GameManager.UIRoot);
            tempImage.raycastTarget = false;
            tempImage.transform.SetAsLastSibling();
            icon.enabled = false;
        }
        #endregion
    }
}
