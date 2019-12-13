using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public abstract class Controller : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Control")]
        [SerializeField] protected bool controlledByPlayer;
        #endregion

        #region Public Properties
        public bool ControlledByPlayer => controlledByPlayer;
        public bool IsControlled => InputManager.CurrentController == this;
        public Motor Motor { get; protected set; }
        public GameAction CurrentAction { get; protected set; }
        public bool CanMove => Motor != null;
        #endregion

        #region Public Methods
        public virtual void OnControlStart()
        {
            AddListeners();
        }

        public virtual void OnControlStop()
        {
            RemoveListeners();
        }

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

        public void StopAction()
        {
            if (CurrentAction == null) return;

            StopAllCoroutines();
            CurrentAction = null;
        }
        #endregion

        #region Private Methods
        protected virtual void AddListeners() { }
        protected virtual void RemoveListeners() { }

        protected virtual void Awake()
        {
            Motor = GetComponent<Motor>();
        }
        #endregion
    }
}
