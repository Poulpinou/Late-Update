using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class TooltipContent : TooltipTarget, ITooltipable
    {
        [SerializeField] string tooltipText;
        [SerializeField] int priority;

        public new string TooltipText
        {
            get => string.IsNullOrEmpty(tooltipText) ? name : tooltipText;
            set => tooltipText = value;
        }

        public int Priority => priority;
    }
}
