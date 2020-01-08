using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class Character : Actor, IInteractable
    {
        public new CharacterStats Stats => base.Stats as CharacterStats;

        public virtual List<GameAction> GetPossibleActions(Actor actor)
        {
            return new List<GameAction> { new Follow_Action(actor, this) };
        }

        protected override void Awake()
        {
            base.Awake();

            Motor.Agent.speed = Stats.RunSpeed.Value;
            Stats.RunSpeed.onStatChanged.AddListener(OnRunSpeedValueChanged);
        }

        void OnRunSpeedValueChanged(ModifiableStat stat)
        {
            Motor.Agent.speed = stat.Value;
        }
    }
}
