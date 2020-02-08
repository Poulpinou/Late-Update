using System;

namespace LateUpdate.Actions {
    public class Haul_Action : GameAction
    {
        string actionName;

        public override string Name => actionName;
        public Inventory SourceInventory { get; private set; }
        public Inventory TargetInventory { get; private set; }
        public Item ItemToHaul { get; private set; }

        public Haul_Action(Actor actor, Storage storage, Item itemToHaul, Inventory sourceInventory = null, Inventory targetInventory = null) : base(actor, storage)
        {
            ItemToHaul = itemToHaul;
            actionName = string.Format("Haul {0} to", itemToHaul.itemName);

            if(sourceInventory != null)
            {
                SourceInventory = sourceInventory;
            }
            else
            {
                SourceInventory = actor.GetComponent<Inventory>();
                if (SourceInventory == null)
                    throw new Exception("Impossible to get " + actor.Infos.name + "'s inventory");
            }

            if (targetInventory != null)
            {
                TargetInventory = targetInventory;
            }
            else
            {
                TargetInventory = storage.Inventory;
                if (TargetInventory == null)
                    throw new Exception("Impossible to get " + storage.name + "'s inventory");
            }
        }

        protected override void OnStart()
        {
            ItemData itemData = SourceInventory.GetDataFromItem(ItemToHaul);
            TargetInventory.AddMax(ref itemData);
        }

        protected override bool OnDoneCheck()
        {
            return true;
        }
    }
}
