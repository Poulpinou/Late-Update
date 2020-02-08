using System.Collections;
using System.Collections.Generic;
using LateUpdate.Actions;
using UnityEngine;

namespace LateUpdate {
    [RequireComponent(typeof(DropContainer))]
    public class Harvestable : Interactable
    {
        [SerializeField][Min(0.1f)] float harvestHardness = 1;
        [SerializeField] int harvestableAmount;
        [SerializeField] Item[] requiredItems;

        public DropContainer DropContainer { get; protected set; }

        public override List<GameAction> GetPossibleActions(Actor actor)
        {
            List<GameAction> actions = new List<GameAction>();

            bool canExtract = false;
            if(requiredItems.Length > 0)
            {
                Inventory inventory = actor.GetComponent<Inventory>();
                for (int i = 0; i < requiredItems.Length; i++)
                {
                    if (inventory.Contains(requiredItems[i]))
                    {
                        canExtract = true;
                        break;
                    }
                }
            }
            else
            {
                canExtract = true;
            }

            if (canExtract)
            {
                actions.Add(new Harvest_Action(actor, this));
            }

            return actions;
        }

        public float ComputeHarvestSpeed(int harvestStat)
        {
            return harvestHardness / (harvestStat * 0.1f);
        }

        public bool Harvest(ref Inventory collector)
        {
            Item item = DropContainer.DropItem();
            if (collector.Add(item))
            {
                harvestableAmount--;
                UpdateDisplay();

                return harvestableAmount > 0;
            }
            return false;
        }

        protected override void Awake()
        {
            base.Awake();
            DropContainer = GetComponent<DropContainer>();
        }

        private void UpdateDisplay()
        {
            OnUpdateDisplay();
            if (harvestableAmount <= 0)
                OnStockOver();
        }

        protected virtual void OnUpdateDisplay() { }
        protected virtual void OnStockOver()
        {
            Destroy(gameObject);
        }
    }
}
