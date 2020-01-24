using UnityEngine;
using System;

namespace LateUpdate {
    [Serializable]
    public struct DropChance
    {
        public Item item;
        [Range(0.001f, 1)] public float weight;
    }
}
