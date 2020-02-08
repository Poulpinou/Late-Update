using System.Collections;
using UnityEngine;
using LateUpdate.Stats;

namespace LateUpdate.Actions {
    public class Follow_Action : GameAction
    {
        public override string Name => "Follow";

        public override bool NeedsContact => false;

        public Follow_Action(Actor actor, IInteractable interactable) : base(actor, interactable) { }

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

        protected override void OnRun()
        {
            Actor.Motor.GoTo(Target);
        }

        protected override void InitTrainers()
        {
            base.InitTrainers();
            StatContainer statContainer = Actor.GetComponent<StatContainer>();

            trainers.Add(new Trainer(statContainer.GetStat<Athletic_Stat>(), 1));
        }

        protected override bool OnDoneCheck()
        {
            return false;
        }
    }
}
