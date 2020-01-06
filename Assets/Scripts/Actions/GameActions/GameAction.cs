using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    /// <summary>
    /// This is the base class for every <see cref="GameAction"/>
    /// </summary>
    public abstract class GameAction
    {
        public enum ExitStatus { done, stopped, hasNextAction }

        Action<ExitStatus> callback;

        #region Public Properties
        /// <summary>
        /// The <see cref="Actor"/> that will perform the action
        /// </summary>
        public Actor Actor { get; protected set; }
        /// <summary>
        /// The <see cref="IInteractable"/> that will be the target of the action
        /// </summary>
        public IInteractable Target { get; protected set; }
        /// <summary>
        /// The displayed name of the action
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// If true, the controller will walk to the target if it can move
        /// </summary>
        public virtual bool NeedsContact => true;
        /// <summary>
        /// If false, the <see cref="Actor"/> and the <see cref="Target"/> should be different
        /// </summary>
        public virtual bool CanApplyOnSelf => false;
        /// <summary>
        /// Returns true if the action is valid
        /// </summary>
        public virtual bool IsValid => CanApplyOnSelf || Target.gameObject != Actor.gameObject;
        
        public GameAction NextAction { get; protected set; }

        public bool HasNextAction => NextAction != null;
        #endregion

        #region Constructors
        /// <summary>
        /// Overide this constructor to add other paramas to your custom <see cref="GameAction"/>
        /// </summary>
        /// <param name="actor">The <see cref="Actor"/> that will perform the action</param>
        /// <param name="target">The <see cref="IInteractable"/> that will be the target of the action</param>
        public GameAction(Actor actor, IInteractable target)
        {
            Actor = actor;
            Target = target;
        }

        public GameAction(Actor actor, IInteractable target, GameAction nextAction) : this(actor, target)
        {
            NextAction = nextAction;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Call this method to tell the <see cref="Actor"/> to perform this action
        /// </summary>
        public void Run()
        {
            Actor.PerformAction(this);
        }

        public virtual IEnumerator Execute(Action<ExitStatus> callback)
        {
            this.callback = callback;
            yield return OnExecute();

            Stop(HasNextAction? ExitStatus.hasNextAction : ExitStatus.done);
        }

        public virtual void Stop(ExitStatus exitStatus = ExitStatus.stopped)
        {
            OnDone(exitStatus);
            callback.Invoke(exitStatus);
        }

        protected abstract void OnDone(ExitStatus exitStatus);

        /// <summary>
        /// Override this method with your custom action's behaviour
        /// </summary>
        protected abstract IEnumerator OnExecute();

        public override string ToString()
        {
            return string.Format("{0} ({1} => {2})", Name, Actor.gameObject.name, Target.gameObject.name);
        }
        #endregion
    }
}
