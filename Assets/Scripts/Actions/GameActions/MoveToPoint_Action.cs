using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    public class MoveToPoint_Action : GameAction
    {
        Vector2 point;

        public override string Name => "Move to";

        public override bool NeedsContact => false;

        public MoveToPoint_Action(Actor actor, Vector2 point) : base(actor, null) {
            this.point = point;
        }

        protected override IEnumerator OnExecute()
        {
            if (!Actor.CanMove)
            {
                Actor.StopAction();
                Debug.Log(string.Format("{0} can't move", Actor.Infos.name));
            }

            yield return Actor.Motor.ReachPoint(point);
        }

        protected override void OnDone(ExitStatus exitStatus)
        {
        }
    }
}
