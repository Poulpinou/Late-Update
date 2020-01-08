using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public abstract class TrainableStat : ModifiableStat
    {
        [SerializeField] [Range(0.1f, 2f)] float expRate = 1;

        int currentExp = 0;

        public virtual float MaxValue => 100;
        public virtual float MinValue => 1;
        public int CurrentExp => currentExp;
        public int ExpToNext => Mathf.RoundToInt(expRate * Mathf.Pow(BaseValue, 3) + 10);
        public float ExpRatio => (float)CurrentExp / ExpToNext;

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

        protected override float CalculateFinalValue()
        {
            baseValue = Mathf.Clamp(baseValue, MinValue, MaxValue);
            return base.CalculateFinalValue();
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} (+{2}) | exp : {3}/{4}xp", Name, Value, Value - BaseValue, CurrentExp, ExpToNext);
        }
    }
}
