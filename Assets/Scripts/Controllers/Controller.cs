using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// This is the base class for every controllable gameObject
    /// </summary>
    public abstract class Controller : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Control")]
        [SerializeField] protected bool controlledByPlayer;
        #endregion

        #region Public Properties
        /// <summary>
        /// True if player can take control of this <see cref="Controller"/>   /!\ TEMPORARY /!\
        /// </summary>
        public bool ControlledByPlayer => controlledByPlayer;
        /// <summary>
        /// True if is controlled by player
        /// </summary>
        public bool IsControlled => InputManager.CurrentController == this;
        /// <summary>
        /// Reference to <see cref="Motor"/> if it has one
        /// </summary>
        public Motor Motor { get; protected set; }
        /// <summary>
        /// The <see cref="GameAction"/> currently performed by this <see cref="Controller"/>
        /// </summary>
        public GameAction CurrentAction { get; protected set; }
        /// <summary>
        /// False if <see cref="Controller"/> has no <see cref="Motor"/>
        /// </summary>
        public bool CanMove => Motor != null;
        #endregion

        #region Public Methods
        /// <summary>
        /// This method is called when control starts
        /// </summary>
        public virtual void OnControlStart()
        {
            AddListeners();
        }

        /// <summary>
        /// This method is called when control stops
        /// </summary>
        public virtual void OnControlStop()
        {
            RemoveListeners();
        }

        /// <summary>
        /// Tells to the <see cref="Controller"/> which <see cref="GameAction"/> it should perform (it will cancel the previous one if any)
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

        #region Private Methods
        protected abstract void AddListeners();
        protected abstract void RemoveListeners();
        #endregion

        #region Runtime Methods
        protected virtual void Awake()
        {
            Motor = GetComponent<Motor>();
        }
        #endregion
    }
}
