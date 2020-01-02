using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class Character : Actor, IInteractable
    { 
        public virtual List<GameAction> GetPossibleActions(Actor actor)
        {
            return new List<GameAction> { new Follow_Action(actor, this) };
        }

        #region Game Actions
        public class Follow_Action : GameAction
        {
            public override string Name => "Follow";

            public override bool NeedsContact => false;

            public Follow_Action(Actor actor, IInteractable interactable) : base(actor, interactable) { }

            public override void Execute()
            {
                Actor.Motor.FollowTarget(Target);
            }
        }
        #endregion
    }
}
