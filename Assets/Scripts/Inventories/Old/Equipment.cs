using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace LateUpdate.Old {
    [RequireComponent(typeof(Inventory))]
    public class Equipment : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] SkinnedMeshRenderer targetMesh;
        #endregion

        #region Private Fields
        EquipableItem[] currentEquipment;
        SkinnedMeshRenderer[] currentMeshes;
        Inventory inventory;
        #endregion

        #region Events
        [Serializable] public class EquipmentEvent : UnityEvent { }

        public EquipmentEvent onEquipmentChanged = new EquipmentEvent();
        #endregion

        #region Public Methods
        public void Equip(EquipableItem equipment)
        {
            int slotIndex = (int)equipment.equipSlot;

            if (currentEquipment[slotIndex] != null)
            {
                inventory.Add(currentEquipment[slotIndex]);
            }

            currentEquipment[slotIndex] = equipment;
            SkinnedMeshRenderer newMesh = Instantiate(equipment.skinnedMesh, targetMesh.transform);
            newMesh.bones = targetMesh.bones;
            newMesh.rootBone = targetMesh.rootBone;
            currentMeshes[slotIndex] = newMesh;


            onEquipmentChanged.Invoke();
        }

        public void Unequip(int slot)
        {
            if (currentEquipment[slot] != null)
            {
                if(currentMeshes[slot] != null)
                {
                    Destroy(currentMeshes[slot].gameObject);
                }

                if (inventory.Add(currentEquipment[slot])) {
                    currentEquipment[slot] = null;
                    onEquipmentChanged.Invoke();
                }   
            }
        }

        public void Unequip(EquipmentSlot slot)
        {
            Unequip((int)slot);
        }

        public void Unequip(EquipableItem item)
        {
            Unequip(item.equipSlot);
        }

        public void UnequipAll()
        {
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                Unequip(i);
            }
        }
        #endregion

        #region Runtime Methods
        private void Start()
        {
            inventory = GetComponent<Inventory>();
            int numSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new EquipableItem[numSlots];
            currentMeshes = new SkinnedMeshRenderer[numSlots];
        }
        #endregion

        
    }
}
