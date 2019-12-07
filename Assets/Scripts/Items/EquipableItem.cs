using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TinyRPG {
    [CreateAssetMenu(menuName = "TinyRPG/Items/EquipableItem", fileName = "New Equipable Item")]
    public class EquipableItem : Item
    {
        #region Public Fields
        [Header("Equipment Settings")]
        public SkinnedMeshRenderer skinnedMesh;
        public EquipmentSlot equipSlot;
        public int armorModifier;
        public int damageModifier;
        #endregion

        #region Public Methods
        public override void Use()
        {
            Equipment equipment = InputManager.CurrentController.GetComponent<Equipment>();
            equipment.Equip(this);
            RemoveFromInventory();
        }
        #endregion

#if UNITY_EDITOR
        #region Editor Methods
        [ContextMenu("Generate Pickable")]
        void GeneratePickable()
        {
            SkinnedMeshRenderer skinnedMeshRenderer = Instantiate(skinnedMesh);
            GameObject go = skinnedMeshRenderer.gameObject;
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();

            meshFilter.sharedMesh = skinnedMeshRenderer.sharedMesh;
            meshRenderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;

            DestroyImmediate(skinnedMeshRenderer);

            Pickable pickable = go.AddComponent<Pickable>();
            pickable.SetItem(this);

            go.AddComponent<BoxCollider>();

            Selection.activeObject = go;
        }
        #endregion
#endif
    }

    public enum EquipmentSlot { Weapon, Head, Chest, Legs, Feet }
}
