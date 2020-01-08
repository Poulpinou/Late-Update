using UnityEngine;

namespace LateUpdate.Stats
{
    /// <summary>
    /// A <see cref="Stat"/> with a <see cref="ModifiableStat.BaseValue"/> that can be updated by experience
    /// </summary>
    public abstract class TrainableStat : ModifiableStat
    {
        #region Serialized Fields
        [Tooltip("Higher is this value, higher is the amount of exp required to level up")]
        [SerializeField] [Range(0.1f, 2f)] float expRate = 1;
        #endregion

        #region Private Fields
        int currentExp = 0;
        #endregion

        #region Public Properties
        /// <summary>
        /// The max level of the <see cref="ModifiableStat.BaseValue"/>
        /// </summary>
        public virtual float MaxValue => 100;
        /// <summary>
        /// The min level of the <see cref="ModifiableStat.BaseValue"/>
        /// </summary>
        public virtual float MinValue => 1;
        /// <summary>
        /// The current amount of experience
        /// </summary>
        public int CurrentExp => currentExp;
        /// <summary>
        /// The required amount of experience for next level
        /// </summary>
        public int ExpToNext => Mathf.RoundToInt(expRate * Mathf.Pow(BaseValue, 3) + 10);
        /// <summary>
        /// The ratio of current exp on required exp
        /// </summary>
        public float ExpRatio => (float)CurrentExp / ExpToNext;
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds <paramref name="amount"/> experience point to this <see cref="Stat"/>
        /// </summary>
        /// <param name="amount">The amount of experience points to add</param>
        public void AddExp(int amount)
        {
            if (baseValue >= MaxValue) return;

            currentExp += amount;
            bool statChanged = false;
            while (currentExp >= ExpToNext)
            {
                currentExp -= ExpToNext;
                baseValue++;
                statChanged = true;
            }

            if(statChanged)
                onStatChanged.Invoke(this);
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} (+{2}) | exp : {3}/{4}xp", Name, Value, Value - BaseValue, CurrentExp, ExpToNext);
        }
        #endregion

        #region Private Methods
        protected override float CalculateFinalValue()
        {
            baseValue = Mathf.Clamp(baseValue, MinValue, MaxValue);
            return base.CalculateFinalValue();
        }
        #endregion
    }
}
