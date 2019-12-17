using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    [Serializable]
    public class ItemData
    {
        [SerializeField] protected Item item;
        [SerializeField] protected int amount;

        public Item Item => item;
        public virtual int Amount { get => amount; set => amount = value; }
        public virtual float Encumbrance => item.encumbrance * amount;

        public ItemData(Item item, int amount = 1)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}
