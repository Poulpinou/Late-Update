using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    public class MoveTo_Action : GameAction
    {
        public override string Name => "Move to";

        public override bool NeedsContact => false;

        public MoveTo_Action(Actor actor, IInteractable target, GameAction nextAction = null) : base(actor, target, nextAction) { }

        protected override IEnumerator OnExecute()
        {
            if (!Actor.CanMove)
            {
                Actor.StopAction();
                Debug.Log(string.Format("{0} can't move", Actor.Infos.name));
            }

            yield return Actor.Motor.ReachTarget(Target);
        }

        protected override void OnDone(ExitStatus exitStatus)
        {
            Actor.Motor.Clear();
        }
    }
}
