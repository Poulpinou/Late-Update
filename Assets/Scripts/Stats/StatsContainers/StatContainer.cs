using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEngine.Events;

namespace LateUpdate {
    public abstract class StatContainer : MonoBehaviour
    {
        [SerializeField] bool trainable = true;

        public Stat[] All { get; protected set; }
        public bool Trainable => trainable;

        public virtual void InitStats()
        {
            InitArrays();
            InitLinkedStats();
        }

        public TStat GetStat<TStat>(string name = null) where TStat : Stat
        {
            return All.Where(s => s is TStat &&(string.IsNullOrEmpty(name) || s.Name == name)).FirstOrDefault() as TStat;
        }

        public TStat[] GetStats<TStat>() where TStat : Stat
        {
            return All.Where(s => s is TStat).Cast<TStat>().ToArray();
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
    }
}
