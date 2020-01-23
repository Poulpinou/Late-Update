using System.Collections;
using UnityEngine;
using LateUpdate.Stats;

namespace LateUpdate.Actions {
    public class MoveTo_Action : GameAction
    {
        public override string Name => "Move to";

        public override bool NeedsContact => false;

        public MoveTo_Action(Actor actor, IInteractable target, GameAction nextAction = null) : base(actor, target, nextAction) { }

        protected override void OnStart()
        {
            if (!Actor.CanMove)
            {
                Actor.StopAction();
                Debug.Log(string.Format("{0} can't move", Actor.Infos.name));
            }
            Actor.Motor.GoTo(Target);
        }

        protected override void OnDone(ExitStatus exitStatus)
        {
            Actor.Motor.StopFollowingTarget();
        }

        protected override bool OnDoneCheck()
        {
            return Target.CanInteract(Actor);
        }

        protected override void InitTrainers()
        {
            base.InitTrainers();
            StatContainer statContainer = Actor.GetComponent<StatContainer>();

            trainers.Add(new Trainer(statContainer.GetStat<Athletic_Stat>(), 1));
        }
    }
}
