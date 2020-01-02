using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public interface ITooltipable
    {
        string TooltipText { get; }
        int Priority { get; }
    }
}
