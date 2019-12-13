using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LateUpdate {
    public class ActionManager : StaticManager<ActionManager>
    {
        [Header("Relations")]
        [SerializeField] ActionPopup popupModel;

        public static List<GameAction> ExtractActions(Controller actor, IInteractable target)
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

        public static ActionPopup OpenActionPopup(Controller actor, IInteractable target, Vector2? screenPosition = null)
        {
            List<GameAction> actions = ExtractActions(actor, target);
            if (actions.Count == 0) return null;

            Vector2 pos = screenPosition.HasValue ? screenPosition.Value : (Vector2)Input.mousePosition;

            ActionPopup popup = Instantiate(Active.popupModel, GameManager.UIRoot);
            popup.Configure(actions, pos);

            return popup;
        }
    }
}
