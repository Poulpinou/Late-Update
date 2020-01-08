using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace LateUpdate.Stats {
    /// <summary>
    /// Attach a child of this component to and object to provide it some stats
    /// </summary>
    public abstract class StatContainer : MonoBehaviour
    {
        #region Serialized Fields
        [Tooltip("If true, the stats of this container will be automatically trained on action")]
        [SerializeField] bool trainable = true;
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns all <see cref="Stat"/> of this container
        /// </summary>
        public Stat[] All { get; protected set; }
        /// <summary>
        /// Returns true if <see cref="TrainableStat"/> in this container can be trained
        /// </summary>
        public bool Trainable => trainable;
        #endregion

        #region Public Methods
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
        #endregion

        #region Private Methods
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
        #endregion
    }
}
