using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// This interface makes a class interactable
    /// </summary>
    public interface IInteractable
    {
        Transform InteractionTransform { get; }
        float InteractionRadius { get; }
        GameObject gameObject { get; }

        List<GameAction> GetPossibleActions(Actor actor);
    }
}
