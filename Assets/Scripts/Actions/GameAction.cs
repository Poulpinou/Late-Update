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
        #region Public Properties
        /// <summary>
        /// The <see cref="Controller"/> that will perform the action
        /// </summary>
        public Controller Actor { get; protected set; }
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
        public abstract bool NeedsContact { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Overide this constructor to add other paramas to your custom <see cref="GameAction"/>
        /// </summary>
        /// <param name="actor">The <see cref="Controller"/> that will perform the action</param>
        /// <param name="target">The <see cref="IInteractable"/> that will be the target of the action</param>
        public GameAction(Controller actor, IInteractable target)
        {
            Actor = actor;
            Target = target;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Call this method to tell the <see cref="Actor"/> to perform this action
        /// </summary>
        public void Run()
        {
            Actor.SetAction(this);
        }

        /// <summary>
        /// Override this method with your custom action's behaviour
        /// </summary>
        public abstract void Execute();

        public override string ToString()
        {
            return string.Format("{0} ({1} => {2})", Name, Actor.gameObject.name, Target.gameObject.name);
        }
        #endregion
    }
}
