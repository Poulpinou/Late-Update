using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class StaticManager<TManager> : MonoBehaviour where TManager : StaticManager<TManager>
    {
        #region Serialized Fields
        [Header("Manager Settings")]
        [SerializeField] protected bool dontDestroyOnLoad = false;
        #endregion

        #region Static Properties
        public static TManager Active { get; private set; }
        #endregion

        #region Private Methods
        protected virtual void Initialize()
        {
            TManager manager = GetComponent<TManager>();

            if (Active != null && Active != manager)
            {
                if (dontDestroyOnLoad)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(Active.gameObject);
                }
            }

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(manager);

            Active = manager;
        }
        #endregion

        #region Runtime Methods
        protected virtual void Awake()
        {
            Initialize();
        }
        #endregion
    }
}
