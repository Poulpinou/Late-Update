using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LateUpdate {
    public class Pickable : Interactable, ITooltipable
    {
        #region Serialized Fields
        [Header("Pickable")]
        [SerializeField] ItemData itemData;
        #endregion

        #region Public Properties
        public ItemData ItemData => itemData;
        public virtual string TooltipText => itemData.ToString();
        public virtual int Priority => 0;
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
    }
}
