using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    public abstract class GameAction
    {
        public Controller Actor { get; protected set; }
        public IInteractable Target { get; protected set; }
        public abstract string Name { get; }
        public abstract bool NeedsContact { get; }

        public GameAction(Controller actor, IInteractable target)
        {
            Actor = actor;
            Target = target;
        }

        public void Run()
        {
            Actor.SetAction(this);
        }

        public abstract void Execute();

        public override string ToString()
        {
            return string.Format("{0} ({1} => {2})", Name, Actor.gameObject.name, Target.gameObject.name);
        }
    }
}
