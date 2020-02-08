using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    [RequireComponent(typeof(WorldObject))]
    public class WorldObjectComponent : MonoBehaviour
    {
        public virtual bool Unique => true;
        public WorldObject Owner { get; private set; }

        protected virtual void Awake()
        {
            Owner = GetComponent<WorldObject>();
        }

        protected virtual void Reset()
        {
            if (Unique)
            {
                WorldObjectComponent[] previous = GetComponents<WorldObjectComponent>();
                for (int i = 0; i < previous.Length; i++)
                {
                    if (previous[i] != this)
                    {
                        string message = string.Format(
                            "Can't add more than one {0} component to a WorldObject",
                            previous.GetType().Name
                        );
                        DestroyImmediate(this);
                        throw new Exception(message);
                    }
                }
            }
        }
    }
}
