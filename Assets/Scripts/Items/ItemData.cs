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

        public ItemData Take(int amount)
        {
            if (amount > this.amount)
                throw new Exception("Impossible to take that much!");

            this.amount -= amount;

            ItemData datas = item.CreateDefaultDatas();
            datas.Amount = amount;

            return datas;
        }

        public Pickable SpawnItem(Vector3 position)
        {
            GameObject go = GameObject.Instantiate(Item.model, GameManager.WorldObjectsRoot);
            Collider collider = go.GetComponent<Collider>();
            if (collider == null)
                collider = go.AddComponent<BoxCollider>();

            float offset = collider.bounds.size.y / 2;
            go.transform.position = position + new Vector3(0, offset, 0);

            InteractionAreaProvider areaProvider = go.AddComponent<InteractionAreaProvider>();
            areaProvider.FetchRadiusToCollider(collider);

            Pickable pickable = go.AddComponent<Pickable>();
            pickable.SetItemDatas(this);

            return pickable;
        }
    }
}
