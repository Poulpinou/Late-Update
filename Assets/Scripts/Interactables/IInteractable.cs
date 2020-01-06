using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// This interface makes a class interactable
    /// </summary>
    public interface IInteractable
    {
        GameObject gameObject { get; }

        List<GameAction> GetPossibleActions(Actor actor);
    }

    public static class InteractableExtentions
    {
        public static InteractionArea GetInteractionArea(this IInteractable interactable)
        {
            InteractionAreaProvider areaProvider = interactable.gameObject.GetComponent<InteractionAreaProvider>();
            if (areaProvider != null)
                return areaProvider.InteractionArea;

            InteractionArea area = new InteractionArea
            {
                point = interactable.gameObject.transform,
                radius = 1
            };

            Collider collider = interactable.gameObject.GetComponent<Collider>();
            if(collider != null)
                area.ComputeRadiusFromCollider(collider, 1);

            return area;
        }

        public static bool CanInteract(this IInteractable interactable, Actor actor)
        {
            return interactable.GetInteractionArea().PointIsInArea(actor.transform.position);
        }
    }
}
