using System;

namespace LateUpdate.Stats {
    [Serializable]
    public class Harvest_Stat : TrainableStat
    {
        public override string Name => "Harvest";

        public override string ShortName => "HAR";

        public override StatCategory Category => StatCategory.Production;
    }
}
