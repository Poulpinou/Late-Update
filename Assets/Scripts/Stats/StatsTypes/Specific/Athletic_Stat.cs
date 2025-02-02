﻿using System;

namespace LateUpdate.Stats {
    [Serializable]
    public class Athletic_Stat : TrainableStat
    {
        public override string Name => "Athletic";

        public override string ShortName => "ATH";

        public override StatCategory Category => StatCategory.Base;
    }
}
