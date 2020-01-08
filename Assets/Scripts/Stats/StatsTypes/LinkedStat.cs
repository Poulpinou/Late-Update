namespace LateUpdate {
    public abstract class LinkedStat : ModifiableStat
    {
        public override float Value => ComputeLinkedValue();
        public Stat[] LinkedStats { get; protected set; }

        protected abstract float ComputeLinkedValue();

        public LinkedStat(params Stat[] stats)
        {
            LinkedStats = stats;
            for (int i = 0; i < LinkedStats.Length; i++)
            {
                ModifiableStat stat = LinkedStats[i] as ModifiableStat;
                if (stat != null)
                {
                    stat.onStatChanged.AddListener(OnLinkedStatChanged);
                }
            }
        }

        protected virtual void OnLinkedStatChanged(ModifiableStat stat)
        {
            onStatChanged.Invoke(this);
        }
    }
}
