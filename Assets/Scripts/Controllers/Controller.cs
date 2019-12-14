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
        /// <summary>
        /// True if player can take control of this <see cref="Controller"/>   /!\ TEMPORARY /!\
        /// </summary>
        public bool ControlledByPlayer => controlledByPlayer;
        /// <summary>
        /// True if is controlled by player
        /// </summary>
        public bool IsControlled => InputManager.CurrentController == this;       
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
        #endregion

        #region Private Methods
        protected abstract void AddListeners();
        protected abstract void RemoveListeners();
        #endregion

        
    }
}
