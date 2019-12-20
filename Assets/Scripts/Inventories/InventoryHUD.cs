using UnityEngine;

namespace LateUpdate {
    public class InventoryHUD : InventoryPanel
    {
        #region Serialized Fields
        [Header("Other relations")]
        [SerializeField] DropHandler trashHandler;
        #endregion

        #region Private Variables
        ItemData tempDatas;
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
            Inventory.Drop(tempDatas.Item, amount);
            tempDatas = null;
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
