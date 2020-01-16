using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate.Stats {
    public class LineOfSight_Stat : LinkedStat
    {
        public const float MIN_VALUE = 3f;
        public const float MAX_VALUE = 50f;

        Perception_Stat perceptionStat;

        public override string Name => "Line of sight";

        public override string ShortName => "los";

        public override StatCategory Category => StatCategory.Movement;

        public override string Unit => "m";

        public LineOfSight_Stat(Perception_Stat perceptionStat) : base(perceptionStat)
        {
            this.perceptionStat = perceptionStat;
        }

        protected override float ComputeLinkedValue()
        {
            return Mathf.Lerp(MIN_VALUE, MAX_VALUE, perceptionStat.Value / perceptionStat.MaxValue);
        }
    }
}
