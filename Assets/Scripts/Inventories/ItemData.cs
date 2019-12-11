using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

    [Serializable]
    public class ContainerData : ItemData
    {
        public List<ItemData> content;

        public override int Amount => content.Count;
        public override float Encumbrance => content.Sum(c => c.Encumbrance);
        public Container Container => item as Container;

        public ContainerData(Container container, List<ItemData> content = null) : base(container)
        {
            if (content != null)
                this.content = content;
            else
                this.content = new List<ItemData>();
        }
    }
}
