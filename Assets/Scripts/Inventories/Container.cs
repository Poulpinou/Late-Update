using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    [CreateAssetMenu(menuName = "TinyRPG/Items/Container", fileName = "New Container", order = 1)]
    public class Container : Item
    {
        [Header("Container")]
        public bool wearable = true;
        public float capacity = 10;
    }
}
