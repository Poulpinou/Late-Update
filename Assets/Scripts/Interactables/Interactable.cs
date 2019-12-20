using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    [RequireComponent(typeof(InteractionAreaProvider))]
    /// <summary>
    /// Base class for every interactable items
    /// </summary>
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        #region Public Methods
        public abstract List<GameAction> GetPossibleActions(Actor actor);
        #endregion
    }
}
