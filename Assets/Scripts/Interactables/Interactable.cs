using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyRPG {
    /// <summary>
    /// Base class for every interactable items
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Interaction Area")]
        [SerializeField] protected float interactionRadius = 1f;
        [SerializeField] protected Transform interactionTransform;
        #endregion

        #region Private Fields
        protected Transform actor;
        protected bool hasInteracted = false;
        #endregion

        #region Public Properties
        public virtual bool IsFocused => actor != null;
        public virtual Transform InteractionTransform => interactionTransform;
        public virtual float InteractionRadius => interactionRadius;
        #endregion

        #region Public Methods
        public virtual void OnFocused(Transform actor)
        {
            this.actor = actor;
            hasInteracted = false;
        }

        public virtual void OnDefocused()
        {
            actor = null;
            hasInteracted = false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Override this method to define Interaction behaviour
        /// </summary>
        protected virtual void Interact()
        {
            Debug.Log(string.Format("{0} is interacting with {1}", actor.name, name));
        }
        #endregion

        #region Runtime Methods
        protected virtual void Update()
        {
            if (IsFocused && !hasInteracted)
            {
                float distance = Vector3.Distance(actor.position, interactionTransform.position);
                if (distance <= interactionRadius)
                {
                    Interact();
                    hasInteracted = true;
                }
            }
        }

        private void OnDestroy()
        {
            if(actor != null)
                actor.SendMessage("OnTargetDestroyed");
        }

        protected virtual void Awake()
        {
            if (interactionTransform == null)
                interactionTransform = transform;
        }
        #endregion

        #region Editor Methods
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
        }

        protected virtual void Reset()
        {
            interactionTransform = transform;
        }
        #endregion



        

        
    }
}
