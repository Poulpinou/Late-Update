using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class Character : MonoBehaviour, IInteractable
    {
        public Transform InteractionTransform => transform;

        public float InteractionRadius => 0.1f;

        public virtual List<GameAction> GetPossibleActions(Controller actor)
        {
            List<GameAction> actions = new List<GameAction>();

            if(gameObject != actor.gameObject)
                actions.Add(new Follow(actor, this));

            return actions;
        }

        #region Game Actions
        public class Follow : GameAction
        {
            public override string Name => "Follow";

            public override bool NeedsContact => false;

            public Follow(Controller actor, IInteractable interactable) : base(actor, interactable) { }

            public override void Execute()
            {
                Actor.Motor.FollowTarget(Target);
            }
        }
        #endregion
    }
}
