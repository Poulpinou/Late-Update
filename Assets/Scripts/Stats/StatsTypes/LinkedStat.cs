namespace LateUpdate.Stats {
    /// <summary>
    /// This class is a <see cref="Stat"/> that depends from other stats
    /// </summary>
    public abstract class LinkedStat : ModifiableStat
    {
        #region Public Properties
        /// <summary>
        /// The final value with modifications
        /// </summary>
        public override float Value => ComputeLinkedValue();
        /// <summary>
        /// An Array that contains every <see cref="Stat"/> linked to this one
        /// </summary>
        public Stat[] LinkedStats { get; protected set; }
        #endregion

        #region Constructors
        /// <summary>
        /// This Constructor should be overrided as follow : 
        ///    public X_Stat(Y_Stat stat1, Z_Stat stat2, ...) : base(stat1, stat2, ...)
        ///    {
        ///        //What you want to do in this constructor
        ///    }
        /// </summary>
        /// <param name="stats">An array of <see cref="Stat"/> that will be linked to this one</param>
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
        #endregion

        #region Private Methods
        /// <summary>
        /// Override this method to return the <see cref="Value"/>
        /// </summary>
        /// <returns>The computed value of this stat</returns>
        protected abstract float ComputeLinkedValue();

        /// <summary>
        /// This method is called when a linked <see cref="ModifiableStat"/> has been modified
        /// </summary>
        /// <param name="stat">The modified <see cref="Stat"/></param>
        protected virtual void OnLinkedStatChanged(ModifiableStat stat)
        {
            onStatChanged.Invoke(this);
        }
        #endregion
    }
}
