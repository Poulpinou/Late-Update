using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// An extension of <see cref="TooltipTarget"/> with static content
    /// </summary>
    public class TooltipContent : TooltipTarget, ITooltipable
    {
        [Header("Content")]
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
