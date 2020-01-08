using UnityEngine;

namespace LateUpdate {
    public abstract class Stat
    {
        public abstract string Name { get; }
        public abstract string ShortName { get; }

        public abstract float Value { get; }
        public virtual int IntValue => Mathf.FloorToInt(Value);

        public override string ToString()
        {
            return Name + " : " + Value;
        }
    }
}
