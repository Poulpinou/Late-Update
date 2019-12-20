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
        [SerializeField] ItemData itemData;
        #endregion

        #region Public Properties
        public ItemData ItemData => itemData;
        #endregion

        #region Public Methods
        public override List<GameAction> GetPossibleActions(Actor actor)
        {
            List<GameAction> interactions = new List<GameAction>();

            Inventory inventory = actor.GetComponent<Inventory>();
            if (inventory != null && actor.CanMove)
                interactions.Add(new PickUp_Action(actor, this, inventory));

            return interactions;

        }

        public void SetItemDatas(ItemData itemData)
        {
            this.itemData = itemData;
            name = "Pickable_" + itemData.Item.itemName;
        }
        #endregion

        #region Interactions
        public class PickUp_Action : GameAction
        {
            readonly Inventory _inventory;
            readonly Pickable _pickable;

            public override string Name => "Pick up";

            public PickUp_Action(Actor actor, Pickable pickable, Inventory inventory) : base(actor, pickable)
            {
                _inventory = inventory;
                _pickable = pickable;
            }

            public override void Execute()
            {
                if (_inventory.Add(_pickable.ItemData))
                {
                    Destroy(_pickable.gameObject);
                }
            }
        }
        #endregion
    }
}
