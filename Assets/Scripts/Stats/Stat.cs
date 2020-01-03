using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

namespace LateUpdate {
    public abstract class Stat
    {
        public abstract string Name { get; }
        public abstract string ShortName { get; }

        public abstract int Value { get; }

        public override string ToString()
        {
            return Name + " : " + Value;
        }
    }

    public abstract class ModifiableStat : Stat
    {
        [Header("Base Values")]
        [SerializeField] protected int baseValue;

        private readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;
        private bool isDirty = true;
        private float _value;
        private float lastBaseValue = float.MinValue;
        
        public int BaseValue => Mathf.FloorToInt(baseValue);
        public override int Value
        {
            get
            {
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return Mathf.FloorToInt(_value);
            }
        }

        public ModifiableStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public ModifiableStat(int baseValue) : this()
        {
            this.baseValue = baseValue;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}(+{2})", Name, Value, Value - BaseValue);
        }

        public void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        public bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        private int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }

        private float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
    }

    public abstract class TrainableStat : ModifiableStat
    {
        [SerializeField] [Range(0.1f, 2f)] float expRate = 1;

        int currentExp = 0;

        public virtual int MaxValue => 100;
        public virtual int MinValue => 1;
        public int CurrentExp => currentExp;
        public int ExpToNext => Mathf.RoundToInt(expRate * Mathf.Pow(BaseValue, 3) + 10);
        public float ExpRatio => (float)CurrentExp / ExpToNext;

        public void AddExp(int amount)
        {
            currentExp += amount;
            while(currentExp >= ExpToNext)
            {
                currentExp -= ExpToNext;
                baseValue++;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} (+{2}) | exp : {3}/{4}xp", Name, Value, Value - BaseValue, CurrentExp, ExpToNext);
        }
    }

    public abstract class LinkedStat : Stat
    {
         
    }

    [Serializable]
    public class Strength_Stat : TrainableStat
    {
        public override string Name => "Strength";
        public override string ShortName => "STR";
    }

    [Serializable]
    public class Constitution_Stat : TrainableStat
    {
        public override string Name => "Constitution";
        public override string ShortName => "CON";
    }

    public class Life_Stat : LinkedStat
    {
        Constitution_Stat constitution;

        public override string Name => "Life";
        public override string ShortName => "HP";

        public Life_Stat(Constitution_Stat constitution)
        {
            this.constitution = constitution;
        }

        public override int Value => constitution.Value * 10;
    }

}
