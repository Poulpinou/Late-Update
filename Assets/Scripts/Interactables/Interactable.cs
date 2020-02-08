using System.Collections.Generic;
using UnityEngine;
using LateUpdate.Actions;

namespace LateUpdate {
    [RequireComponent(typeof(InteractionAreaProvider))]
    /// <summary>
    /// Base class for every interactable items
    /// </summary>
    public abstract class Interactable : WorldObject, IInteractable, ITooltipable
    {
        public override string[] CollectionTags => new string[]{"Interactable"};

        public virtual string TooltipText => name;

        public int Priority => 0;

        #region Public Methods
        public abstract List<GameAction> GetPossibleActions(Actor actor);
        #endregion
    }
}
