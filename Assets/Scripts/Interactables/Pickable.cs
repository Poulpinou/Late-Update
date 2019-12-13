using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LateUpdate {
    public class Pickable : Interactable
    {
        #region Serialized Fields
        [Header("Pickable")]
        [SerializeField] Item item;
        #endregion

        #region Public Properties
        public Item Item => item;
        #endregion

        #region Public Methods
        public override List<GameAction> GetPossibleActions(Controller controller)
        {
            List<GameAction> interactions = new List<GameAction>();

            Inventory inventory = controller.GetComponent<Inventory>();
            if (inventory != null && controller.CanMove)
                interactions.Add(new PickUp(controller, this, inventory));

            return interactions;

        }
        #endregion

        #region Private Methods
        internal void SetItem(Item item)
        {
            this.item = item;
            name = "Pickable_" + item.itemName;
            item.pickable = this;
        }
        #endregion

        #region Interactions
        public class PickUp : GameAction
        {
            readonly Inventory _inventory;
            readonly Pickable _pickable;

            public override string Name => "Pick up";
            public override bool NeedsContact => true;

            public PickUp(Controller actor, Pickable pickable, Inventory inventory) : base(actor, pickable)
            {
                _inventory = inventory;
                _pickable = pickable;
            }

            public override void Execute()
            {
                if (_inventory.Add(_pickable.Item))
                {
                    Destroy(_pickable.gameObject);
                }
            }
        }
        #endregion

#if UNITY_EDITOR
        #region Editor Methods
        [ContextMenu("Link To Item")]
        void LinkToItem()
        {
            if(PrefabUtility.GetCorrespondingObjectFromSource(gameObject) != null)
            {
                throw new Exception("This method should be called on prefab!");
            }

            item.pickable = this;
        }
        #endregion
#endif
    }
}
