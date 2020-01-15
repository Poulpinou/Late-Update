using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace LateUpdate {
    /// <summary>
    /// Add this to a <see cref="GameObject"/> to draw text in <see cref="Tooltip"/> on mouseover
    /// The text will be defined by every <see cref="ITooltipable"/> on this object
    /// </summary>
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")]
        [Tooltip("If true, the tooltip will hide OnDestroy() and OnDisable()")]
        [SerializeField] bool checkDisable = false;

        public virtual string TooltipText {
            get
            {
                IEnumerable<ITooltipable> tooltipables = GetComponents<ITooltipable>().OrderBy(t => t.Priority);
                string text = "";
                foreach (ITooltipable tooltipable in tooltipables)
                {
                    text += tooltipable.TooltipText;
                }

                return text;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.Show(TooltipText);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.Hide();
        }

        private void OnMouseEnter()
        {
            if(!EventSystem.current.IsPointerOverGameObject())
                Tooltip.Show(TooltipText);
        }

        private void OnMouseExit()
        {
            Tooltip.Hide();
        }

        private void OnDestroy()
        {
            if(checkDisable)
                Tooltip.Hide();
        }

        private void OnDisable()
        {
            if(checkDisable)
                Tooltip.Hide();
        }
    }
}
