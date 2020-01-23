using System.Collections;
using System.Collections.Generic;
using LateUpdate.Actions;
using UnityEngine;

namespace LateUpdate {
    [RequireComponent(typeof(Inventory))]
    public class Storage : Interactable
    {
        [SerializeField] float capacity;

        public Inventory Inventory { get; protected set; }

        public override List<GameAction> GetPossibleActions(Actor actor)
        {
            List<GameAction> actions = new List<GameAction>();

            actions.Add(new Trade_Action(actor, Inventory));

            TypedInventory typedInventory = Inventory as TypedInventory;
            if (typedInventory != null)
            {
                foreach(Item item in typedInventory.AllowedItems)
                {
                    actions.Add(new Haul_Action(actor, this, item));
                }
            }

            return actions;
        }

        protected override void Awake()
        {
            base.Awake();

            Inventory = GetComponent<Inventory>();
            Inventory.Capacity = capacity;
        }
    }
}
