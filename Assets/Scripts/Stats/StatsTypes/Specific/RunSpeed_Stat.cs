using UnityEngine;

namespace LateUpdate.Stats {
    public class RunSpeed_Stat : LinkedStat
    {
        public const float MIN_VALUE = 3f;
        public const float MAX_VALUE = 20f;

        Athletic_Stat athleticStat;

        public override string Name => "Run Speed";

        public override string ShortName => "rspd";

        public override StatCategory Category => StatCategory.Movement;

        public override string Unit => "km/h";

        public RunSpeed_Stat(Athletic_Stat athleticStat) : base(athleticStat)
        {
            this.athleticStat = athleticStat;
        }

        protected override float ComputeLinkedValue()
        {
            return Mathf.Lerp(MIN_VALUE, MAX_VALUE, athleticStat.Value / athleticStat.MaxValue);
        }
    }
}
