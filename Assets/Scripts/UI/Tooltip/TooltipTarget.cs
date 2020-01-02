using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace LateUpdate {
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
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
