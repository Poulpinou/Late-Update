using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System;
using UnityEngine.Events;

namespace LateUpdate.Stats
{
    public abstract class ModifiableStat : Stat
    {
        #region Serialized Fields
        [SerializeField] protected float baseValue;
        #endregion

        #region Private Fields
        private readonly List<StatModifier> statModifiers;
        private bool isDirty = true;
        private float lastValue;
        private float lastBaseValue = float.MinValue;
        #endregion

        #region Public Properties
        /// <summary>
        /// The <see cref="Value"/> without <see cref="StatModifiers"/> effects
        /// </summary>
        public float BaseValue => baseValue;
        /// <summary>
        /// Returns the <see cref="BaseValue"/> floored to int
        /// </summary>
        public int IntBaseValue => Mathf.FloorToInt(BaseValue);
        /// <summary>
        /// The final value with modifications
        /// </summary>
        public override float Value
        {
            get
            {
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    lastValue = CalculateFinalValue();
                    onStatChanged.Invoke(this);
                    isDirty = false;
                }
                return lastValue;
            }
        }
        /// <summary>
        /// Returns the value given by <see cref="StatModifiers"/> only
        /// </summary>
        public float BonusValue => Value - BaseValue;
        /// <summary>
        /// Returns the int value given by <see cref="StatModifiers"/> only
        /// </summary>
        public int IntBonusValue => IntValue - IntBaseValue;
        /// <summary>
        /// A list of every <see cref="StatModifier"/> attached to this <see cref="Stat"/>
        /// </summary>
        public ReadOnlyCollection<StatModifier> StatModifiers { get; protected set; }
        #endregion

        #region Events
        public class StatChangedEvent : UnityEvent<ModifiableStat> { }
        /// <summary>
        /// This <see cref="StatChangedEvent"/> is called when the value of this stat is modified
        /// </summary>
        public StatChangedEvent onStatChanged = new StatChangedEvent();
        #endregion

        #region Constructors
        public ModifiableStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public ModifiableStat(float baseValue) : this()
        {
            this.baseValue = baseValue;
        }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("{0} : {1}(+{2})", Name, Value, Value - BaseValue);
        }

        /// <summary>
        /// Use this to add a <see cref="StatModifier"/> to change the <see cref="Value"/>
        /// </summary>
        /// <param name="mod">The instance of the modifier</param>
        public void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        /// <summary>
        /// Removes <paramref name="mod"/> if one is attached to this <see cref="Stat"/>
        /// </summary>
        /// <param name="mod">The modifier to remove</param>
        /// <returns>True if a modifier has been removed</returns>
        public bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes every modifiers that has <paramref name="source"/> as <see cref="StatModifier.Source"/>
        /// </summary>
        /// <param name="source">The source of the targets</param>
        /// <returns>True if at least one <see cref="StatModifier"/> has been removed</returns>
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
        #endregion

        #region Private Methods
        private int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModifier.ModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModifier.ModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModifier.ModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModifier.ModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
        #endregion
    }
}
