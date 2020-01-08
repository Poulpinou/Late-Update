using UnityEngine;

namespace LateUpdate.Stats {
    /// <summary>
    /// This is the base class for every stats
    /// </summary>
    public abstract class Stat
    {
        /// <summary>
        /// The long name of the <see cref="Stat"/>
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// The short name of the <see cref="Stat"/>
        /// </summary>
        public abstract string ShortName { get; }
        /// <summary>
        /// The float value of the <see cref="Stat"/>
        /// </summary>
        public abstract float Value { get; }
        /// <summary>
        /// The int value of the <see cref="Stat"/>
        /// </summary>
        public virtual int IntValue => Mathf.FloorToInt(Value);

        public override string ToString()
        {
            return Name + " : " + Value;
        }
    }
}
