using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    [RequireComponent(typeof(Actor))]
    public class Equipment : WorldObjectComponent
    {
        EquipableItem weapon;
        EquipableItem head;
        EquipableItem chest;
        EquipableItem legs;
        EquipableItem feet;

        public void Equip(EquipableItem equipable)
        {
            if (!CanEquip(equipable)) return;

            // Do equipment system

            equipable.OnEquip(this);
        }

        public bool CanEquip(EquipableItem equipable) {
            return true;
        }
    }
}
