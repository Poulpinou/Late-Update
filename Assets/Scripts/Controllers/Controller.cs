using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// This class makes the link beetween an object and the <see cref="InputManager"/> and player actions
    /// </summary>
    public abstract class Controller : MonoBehaviour, ICollectionTaggable
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
        public string[] CollectionTags => new string[] { "Controllable" };
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
