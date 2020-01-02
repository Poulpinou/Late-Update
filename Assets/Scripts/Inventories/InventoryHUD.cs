using UnityEngine;

namespace LateUpdate {
    public class InventoryHUD : InventoryPanel
    {
        #region Serialized Fields
        [Header("Other relations")]
        [SerializeField] DropHandler trashHandler;
        #endregion

        #region Public Properties
        public override bool DestroyIfControlChanged => false;
        #endregion

        #region Public Methods
        public override void LinkInventory(Inventory inventory)
        {
            base.LinkInventory(inventory);

            if (Inventory == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
        }
        #endregion

        #region Private Methods
        protected override void OnSlotDrag(ItemData itemData, InventorySlotUI.SlotDragAction dragAction)
        {
            base.OnSlotDrag(itemData, dragAction);
            switch (dragAction)
            {
                case InventorySlotUI.SlotDragAction.startDrag:
                    trashHandler.gameObject.SetActive(true);
                    trashHandler.onDrop.AddListener(delegate {
                        ActionManager.OpenAmountPopup(
                            new AmountCallback<ItemData>(PerformDrop, itemData), 
                            itemData.Amount, 
                            itemData.Amount, 
                            0
                        );
                    });
                    break;
                case InventorySlotUI.SlotDragAction.drag:
                    break;
                case InventorySlotUI.SlotDragAction.endDrag:
                    trashHandler.gameObject.SetActive(false);
                    trashHandler.onDrop.RemoveAllListeners();
                    break;
            }
        }

        void PerformDrop(ItemData target, int amount)
        {
            Inventory.Drop(target.TakeAmount(amount));
        }

        void OnCurrentControllerChanged(Controller controller)
        {
            LinkInventory(controller == null ? null : controller.GetComponent<Inventory>());
        }
        #endregion

        #region Runtime Methods
        private void Start()
        {
            OnCurrentControllerChanged(InputManager.CurrentController);
            InputManager.Active.onCurrentControllerChanged.AddListener(OnCurrentControllerChanged);
        }
        #endregion
    }
}
