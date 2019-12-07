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
        #endregion

        #region Private Methods
        protected virtual void AddListeners() { }
        protected virtual void RemoveListeners() { }
        #endregion
    }
}
