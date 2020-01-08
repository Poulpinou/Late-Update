using System.Collections;
using UnityEngine;

namespace LateUpdate.Actions {
    /// <summary>
    /// This action opens the <see cref="GameAction.Target"/> <see cref="Inventory"/>
    /// </summary>
    public class Trade_Action : GameAction
    {
        Inventory inventory;

        public override string Name => "Trade";

        public Trade_Action(Actor actor, Inventory target) : base(actor, target)
        {
            inventory = target;
        }

        protected override IEnumerator OnExecute()
        {
            UIManager.CreateFloatingPanel<InventoryPanel>(
                Camera.main.WorldToScreenPoint(Target.gameObject.transform.position),
                i => {
                    i.LinkInventory(inventory);
                    i.CloseCondition = IsTooFar;
                }
            );

            yield break;
        }

        bool IsTooFar()
        {
            return Vector3.Distance(Actor.transform.position, Target.gameObject.transform.position) > 5;
        }

        protected override void OnDone(ExitStatus exitStatus)
        {
            
        }
    }
    
}
