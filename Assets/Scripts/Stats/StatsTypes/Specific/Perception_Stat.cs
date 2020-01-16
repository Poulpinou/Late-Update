using System;

namespace LateUpdate.Stats {
    [Serializable]
    public class Perception_Stat : TrainableStat
    {
        public override string Name => "Perception";

        public override string ShortName => "PER";

        public override StatCategory Category => StatCategory.Base;
    }
}
