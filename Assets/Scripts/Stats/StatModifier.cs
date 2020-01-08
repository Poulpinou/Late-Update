using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate.Stats {
    /// <summary>
    /// Add an instance of this to a <see cref="ModifiableStat"/> to modify this stat
    /// </summary>
    public class StatModifier
    {
        #region Enums
        /// <summary>
        /// This enum describes the way the <see cref="StatModifier"/> should be applied
        /// </summary>
        public enum ModType
        {
            /// <summary>
            /// Just add the <see cref="Value"/> to the <see cref="ModifiableStat.Value"/>
            /// </summary>
            Flat = 100,
            /// <summary>
            /// Adds <see cref="Value"/>% of the <see cref="ModifiableStat.BaseValue"/> to <see cref="ModifiableStat.Value"/>
            /// </summary>
            PercentAdd = 200,
            /// <summary>
            /// Adds <see cref="Value"/>% of the <see cref="ModifiableStat.Value"/> to itself
            /// </summary>
            PercentMult = 300,
        }
        #endregion

        #region Public Fields
        /// <summary>
        /// The value of the modification (/!\ Scale depends of <see cref="ModType"/>)
        /// </summary>
        public readonly float Value;
        /// <summary>
        /// The way the <see cref="Value"/> will be applied to <see cref="ModifiableStat"/>
        /// </summary>
        public readonly ModType Type;
        /// <summary>
        /// The apply order, the smallest value will be applied first
        /// </summary>
        public readonly int Order;
        /// <summary>
        /// The origin of this <see cref="StatModifier"/>
        /// </summary>
        public readonly object Source;
        #endregion

        #region Constructors
        public StatModifier(float value, ModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }
        public StatModifier(float value, ModType type) : this(value, type, (int)type, null) { }

        public StatModifier(float value, ModType type, int order) : this(value, type, order, null) { }

        public StatModifier(float value, ModType type, object source) : this(value, type, (int)type, source) { }
        #endregion
    }
}
