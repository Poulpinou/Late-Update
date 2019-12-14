using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LateUpdate {
    /// <summary>
    /// This Manager helps you to configure actions and call them staticaly
    /// </summary>
    public class ActionManager : StaticManager<ActionManager>
    {
        #region Serialized Fields
        [Header("Relations")]
        [SerializeField] ActionPopup popupModel;
        #endregion

        #region Static Methods
        /// <summary>
        /// This method extracts every possible <see cref="GameAction"/> from the actor and the target
        /// </summary>
        /// <param name="actor">The <see cref="Actor"/> that will perform the action</param>
        /// <param name="target">The <see cref="IInteractable"/> that will be the target of the action</param>
        /// <returns>A List of <see cref="GameAction"/></returns>
        public static List<GameAction> ExtractActions(Actor actor, IInteractable target)
        {
            List<GameAction> actions = new List<GameAction>();
            List<IInteractable> interactables = new List<IInteractable>();
            interactables.AddRange(actor.GetComponents<IInteractable>());
            interactables.AddRange(target.gameObject.GetComponents<IInteractable>());

            foreach (IInteractable interactable in interactables)
            {
                foreach(GameAction action in interactable.GetPossibleActions(actor))
                {
                    if (!actions.Any(a => a.Name == action.Name))
                        actions.Add(action);
                }
            }

            return actions;
        }

        /// <summary>
        /// Opens an <see cref="ActionPopup"/> at <paramref name="screenPosition"/> filled with extracted actions from <paramref name="actor"/> and <paramref name="target"/>
        /// </summary>
        /// <param name="actor">The <see cref="Actor"/> that will perform the action</param>
        /// <param name="target">The <see cref="IInteractable"/> that will be the target of the action</param>
        /// <param name="screenPosition">The position on the screen (= mouse position by default)</param>
        /// <returns>The instantiated action popup</returns>
        public static ActionPopup OpenActionPopup(Actor actor, IInteractable target, Vector2? screenPosition = null)
        {
            List<GameAction> actions = ExtractActions(actor, target);
            if (actions.Count == 0) return null;

            Vector2 pos = screenPosition.HasValue ? screenPosition.Value : (Vector2)Input.mousePosition;

            ActionPopup popup = Instantiate(Active.popupModel, GameManager.UIRoot);
            popup.Configure(actions, pos);

            return popup;
        }
        #endregion
    }
}
