using System.Collections;
using System;
using System.Linq;

namespace LateUpdate.Stats {
    [Serializable]
    public class CharacterStats : StatContainer
    {
        //Base Stats
        public Athletic_Stat Athletic;

        //Linked Stats
        public RunSpeed_Stat RunSpeed { get; protected set; }

        public TrainableStat[] AllTrainables { get; protected set; }

        protected override void InitLinkedStats()
        {
            RunSpeed = new RunSpeed_Stat(Athletic);
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
