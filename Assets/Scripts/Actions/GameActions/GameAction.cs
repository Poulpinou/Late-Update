using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LateUpdate.Stats;

namespace LateUpdate.Actions {
    /// <summary>
    /// This is the base class for every <see cref="GameAction"/>
    /// </summary>
    public abstract class GameAction
    {
        #region Enums
        public enum ExitStatus { done, stopped, hasNextAction }
        #endregion

        #region Private Fields
        protected Action<ExitStatus> callback;
        protected List<Trainer> trainers;
        #endregion

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

        public virtual float WaitingTime => ActionManager.DefaultActionUpdateTime;
        public bool IsDone { get; set; }
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
        /// Executes this action and invoke <paramref name="callback"/> when it's done
        /// </summary>
        /// <param name="callback">Action to do when action is done</param>
        public virtual IEnumerator Execute(Action<ExitStatus> callback)
        {
            this.callback = callback;

            StatContainer statContainer = Actor.GetComponent<StatContainer>();
            if(statContainer != null && statContainer.Trainable)
                InitTrainers();

            OnStart();
            while (!IsDone)
            {
                if(trainers != null)
                {
                    for (int i = 0; i < trainers.Count; i++)
                    {
                        trainers[i].Update();
                    }
                    OnTrain();
                }
                OnRun();

                IsDone = OnDoneCheck();
                if (!IsDone)
                    yield return new WaitForSeconds(WaitingTime);
                else
                    yield return null;
            }
            Stop(HasNextAction? ExitStatus.hasNextAction : ExitStatus.done);
        }

        /// <summary>
        /// Stops the action and invoke its callback
        /// </summary>
        /// <param name="exitStatus">The <see cref="ExitStatus"/> to send</param>
        public virtual void Stop(ExitStatus exitStatus = ExitStatus.stopped)
        {
            OnDone(exitStatus);
            callback.Invoke(exitStatus);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} => {2})", Name, Actor.gameObject.name, Target.gameObject.name);
        }
        #endregion

        #region Private Methods
        protected virtual void InitTrainers() {
            trainers = new List<Trainer>();
        }

        protected abstract bool OnDoneCheck();

        protected virtual void OnStart() { }
        protected virtual void OnRun() { }
        protected virtual void OnTrain() { }
        /// <summary>
        /// Override this method if your custom action should do something when it's done
        /// </summary>
        protected virtual void OnDone(ExitStatus exitStatus) { }
        #endregion
    }
}
