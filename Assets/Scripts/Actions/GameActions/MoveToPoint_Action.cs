using System.Collections;
using UnityEngine;

namespace LateUpdate.Actions {
    public class MoveToPoint_Action : GameAction
    {
        Vector3 point;
        bool isDone;

        public override string Name => "Move to";

        public override bool NeedsContact => false;

        public MoveToPoint_Action(Actor actor, Vector3 point) : base(actor, null) {
            this.point = point;
        }

        protected override void OnStart()
        {
            if (!Actor.CanMove)
            {
                Actor.StopAction();
                Debug.Log(string.Format("{0} can't move", Actor.Infos.name));
            }

            Actor.Motor.GoTo(point, () => isDone = true);
        }

        protected override bool OnDoneCheck()
        {
            return isDone;
        }
    }
}
