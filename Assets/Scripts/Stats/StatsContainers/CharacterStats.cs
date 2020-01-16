using System.Collections;
using System;
using System.Linq;

namespace LateUpdate.Stats {
    [Serializable]
    public class CharacterStats : StatContainer
    {
        //Base Stats
        public Athletic_Stat Athletic;
        public Perception_Stat Perception;

        //Linked Stats
        public RunSpeed_Stat RunSpeed;
        public LineOfSight_Stat LineOfSight;

        public TrainableStat[] AllTrainables { get; protected set; }

        protected override void InitLinkedStats()
        {
            RunSpeed = new RunSpeed_Stat(Athletic);
            LineOfSight = new LineOfSight_Stat(Perception);
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
