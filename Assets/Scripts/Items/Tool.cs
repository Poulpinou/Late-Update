using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class Tool : EquipableItem
    {
        [Header("Animations")]
        public AnimationClip useAnimation;

        internal override void OnEquip(Equipment equipment)
        {
            equipment.Owner.DoOnWorldObjectComponent<ActorAnimator>(c => c.OverrideController["useTool"] = useAnimation);
        }

        internal override void OnUnequip(Equipment equipment)
        {
            equipment.Owner.DoOnWorldObjectComponent<ActorAnimator>(c => c.OverrideController["useTool"] = null);
        }
    }
}
