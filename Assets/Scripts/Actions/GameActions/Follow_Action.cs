using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class Follow_Action : GameAction
    {
        public override string Name => "Follow";

        public override bool NeedsContact => false;

        public Follow_Action(Actor actor, IInteractable interactable) : base(actor, interactable) { }

        protected override IEnumerator OnExecute()
        {
            if (!Actor.CanMove)
            {
                Actor.StopAction();
                Debug.Log(string.Format("{0} can't move", Actor.Infos.name));
            }

            yield return Actor.Motor.KeepFollowingTarget(Target);
        }

        protected override void OnDone(ExitStatus exitStatus)
        {
            
        }
    }
}
