using System.Collections;
using UnityEngine;

namespace LateUpdate.Actions {
    /// <summary>
    /// This action opens the <see cref="GameAction.Target"/> <see cref="Inventory"/>
    /// </summary>
    public class Trade_Action : GameAction
    {
        Inventory inventory;
        FloatingWindow window;

        public override string Name => "Trade";

        public Trade_Action(Actor actor, Inventory target) : base(actor, target)
        {
            inventory = target;
        }

        protected override void OnStart()
        {
            window = UIManager.CreateFloatingPanel<InventoryPanel>(
                UIManager.DefaultFloatingWindowPosition,
                i => {
                    i.LinkInventory(inventory);
                    i.CloseCondition = IsTooFar;
                }
            );
        }

        bool IsTooFar()
        {
            return Vector3.Distance(Actor.transform.position, Target.gameObject.transform.position) > 5;
        }

        protected override bool OnDoneCheck()
        {
            return window == null || IsTooFar();
        }
    }
    
}
