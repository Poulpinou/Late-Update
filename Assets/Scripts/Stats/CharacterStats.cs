using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace LateUpdate {
    [Serializable]
    public class CharacterStats : StatContainer
    {
        public Life_Stat life;
        public Strength_Stat strength;
        public Constitution_Stat constitution;

        public TrainableStat[] AllTrainables { get; protected set; }

        protected override void InitLinkedStats()
        {
            life = new Life_Stat(constitution);
        }

        protected override Stat[] InitArrays()
        {
            Stat[] stats = base.InitArrays();

            AllTrainables = stats
                .Where(s => s is TrainableStat)
                .Cast<TrainableStat>()
                .ToArray();

            return stats;
        }
    }
}
