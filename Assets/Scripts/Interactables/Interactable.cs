using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// Base class for every interactable items
    /// </summary>
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        #region Serialized Fields
        [Header("Interaction Area")]
        [SerializeField] protected float interactionRadius = 1f;
        [SerializeField] protected Transform interactionTransform;
        #endregion

        #region Public Properties
        public virtual Transform InteractionTransform => interactionTransform;
        public virtual float InteractionRadius => interactionRadius;
        #endregion

        #region Public Methods
        public abstract List<GameAction> GetPossibleActions(Controller controller);
        #endregion

        #region Runtime Methods
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
