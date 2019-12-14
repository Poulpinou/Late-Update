using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class Actor : MonoBehaviour
    {
        /// <summary>
        /// Reference to <see cref="Motor"/> if it has one
        /// </summary>
        public Motor Motor { get; protected set; }
        /// <summary>
        /// False if <see cref="Controller"/> has no <see cref="Motor"/>
        /// </summary>
        public bool CanMove => Motor != null;
        /// <summary>
        /// The <see cref="GameAction"/> currently performed by this <see cref="Actor"/>
        /// </summary>
        public GameAction CurrentAction { get; protected set; }

        #region Public Methods
        /// <summary>
        /// Tells to the <see cref="Actor"/> which <see cref="GameAction"/> it should perform (it will cancel the previous one if any)
        /// </summary>
        /// <param name="action">The <see cref="GameAction"/> to perform</param>
        public void SetAction(GameAction action)
        {
            StopAction();

            CurrentAction = action;

            if (CurrentAction.NeedsContact == true)
            {
                Motor.GoTo(CurrentAction.Target, CurrentAction.Execute);
            }
            else
            {
                CurrentAction.Execute();
            }
        }

        /// <summary>
        /// Stops the <see cref="CurrentAction"/>
        /// </summary>
        public void StopAction()
        {
            if (CurrentAction == null) return;

            StopAllCoroutines();
            CurrentAction = null;
        }
        #endregion

        #region Runtime Methods
        protected virtual void Awake()
        {
            Motor = GetComponent<Motor>();
        }
        #endregion
    }
}
