using System.Collections;
using UnityEngine;

namespace LateUpdate.Actions {
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

        protected override IEnumerator OnExecute()
        {
            if (_inventory.Add(_pickable.ItemData))
            {
                GameObject.Destroy(_pickable.gameObject);
            }

            yield return null;
        }

        protected override void OnDone(ExitStatus exitStatus)
        {
            
        }
    }
}
