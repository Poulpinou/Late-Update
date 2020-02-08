using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LateUpdate {
    public class DropContainer : MonoBehaviour
    {
        [SerializeField] List<DropChance> dropChances;

        DropChance[] absoluteDropChances;

        public DropChance[] DropChances => dropChances.ToArray();
        public DropChance[] AbsoluteDropChances {
            get
            {
                if(absoluteDropChances == null || absoluteDropChances.Length != dropChances.Count)
                    ComputeAbsoluteDropChances();
                return absoluteDropChances;
            }
        }

        public Item DropItem()
        {
            float rand = Random.value;
            float sum = 0;
            for (int i = 0; i < AbsoluteDropChances.Length; i++)
            {
                sum += AbsoluteDropChances[i].weight;
                if(rand <= sum )
                {
                    return AbsoluteDropChances[i].item;
                }
            }
            return null;
        }

        public List<ItemData> DropItems(int amount)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < amount; i++)
            {
                items.Add(DropItem());
            }

            return items
                .GroupBy(i => i)
                .Select(d => new ItemData(null, d.Key, d.Count()))
                .ToList();
        }

        public void ComputeAbsoluteDropChances()
        {
            absoluteDropChances = new DropChance[dropChances.Count];

            float factor = 1 / dropChances.Sum(a => a.weight);

            for (int i = 0; i < dropChances.Count; i++)
            {
                absoluteDropChances[i] = new DropChance() {
                    item = dropChances[i].item,
                    weight = dropChances[i].weight * factor
                };
            }
        }
    }
}
