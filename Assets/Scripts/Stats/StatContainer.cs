using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace LateUpdate {
    public abstract class StatContainer
    {
        [SerializeField] bool train = true;

        public Stat[] All { get; protected set; }

        public virtual void InitStats()
        {
            InitArrays();
            InitLinkedStats();
        }

        protected virtual Stat[] InitArrays()
        {
            List<Stat> stats = new List<Stat>();

            foreach(FieldInfo field in GetType().GetFields())
            {
                if(typeof(Stat).IsAssignableFrom(field.FieldType))
                {
                    stats.Add(field.GetValue(this) as Stat);
                }
            }

            All = stats.ToArray();
            return All;
        }

        protected abstract void InitLinkedStats();

        void Train(object value)
        {
            int val = (int)value;
            //stats.constitution.AddExp(1);
            //Debug.Log(stats.constitution);
        }
    }
}
