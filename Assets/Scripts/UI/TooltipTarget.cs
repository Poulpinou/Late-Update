using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LateUpdate {
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] string tooltipText;
        [SerializeField] bool checkDisable = false;

        public string TooltipText {
            get => string.IsNullOrEmpty(tooltipText) ? name : tooltipText;
            set => tooltipText = value;
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
