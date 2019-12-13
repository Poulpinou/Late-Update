using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public interface IInteractable
    {
        Transform InteractionTransform { get; }
        float InteractionRadius { get; }
        GameObject gameObject { get; }

        List<GameAction> GetPossibleActions(Controller controller);
    }
}
