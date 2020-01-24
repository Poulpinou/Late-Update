using System.Collections;
using System.Collections.Generic;
using LateUpdate.Actions;
using UnityEngine;

namespace LateUpdate {
    [RequireComponent(typeof(DropContainer))]
    public class Harvestable : Interactable
    {
        [SerializeField] Item[] requiredItems;

        public DropContainer DropContainer { get; protected set; }

        public override List<GameAction> GetPossibleActions(Actor actor)
        {
            throw new System.NotImplementedException();
        }

        protected override void Awake()
        {
            base.Awake();
            DropContainer = GetComponent<DropContainer>();
        }
    }
}
