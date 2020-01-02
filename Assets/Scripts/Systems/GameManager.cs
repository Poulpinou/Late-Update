using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class GameManager : StaticManager<GameManager>
    {
        #region Serialized Fields
        [Header("Parents References")]
        [SerializeField] Transform uIRoot;
        [SerializeField] Transform systemsRoot;
        [SerializeField] Transform worldObjectsRoot;
        #endregion

        #region Static Properties
        public static Transform UIRoot => Active.uIRoot;
        public static Transform SystemsRoot => Active.systemsRoot;
        public static Transform WorldObjectsRoot => Active.worldObjectsRoot;
        #endregion

        #region Static Methods
        public static TManager GetManager<TManager>() where TManager : StaticManager<TManager>
        {
            return SystemsRoot.GetComponentInChildren<TManager>();
        }
        #endregion
    }
}
