using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LateUpdate {
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string tooltipText;
        public bool checkDisable = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.Show(tooltipText);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.Hide();
        }

        private void OnMouseEnter()
        {
            Tooltip.Show(tooltipText);
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
