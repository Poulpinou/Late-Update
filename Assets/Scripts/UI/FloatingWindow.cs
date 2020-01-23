using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace LateUpdate {
    public class FloatingWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Button closeButton;
        [SerializeField] Text panelName;

        Vector2 offset;

        public UIPanel Panel { get; private set; }
        public bool IsDragging { get; private set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(eventData.rawPointerPress != Panel.gameObject)
            {
                offset = transform.position - Input.mousePosition;
                IsDragging = true;
            }
            else
            {
                eventData.pointerDrag = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(IsDragging)
                transform.position = (Vector2)Input.mousePosition + offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            offset = Vector2.zero;
            IsDragging = false;
        }

        public void AppendPanel(UIPanel panel)
        {
            if(Panel != null)
                throw new Exception(string.Format("Impossible to set {0}, {1} already contains {2}", panel.name, name, Panel.name));

            Panel = panel;

            GetComponent<RectTransform>().sizeDelta = panel.RectTransform.sizeDelta;
            Panel.transform.parent = transform;
            Panel.transform.localPosition = Vector2.zero;
            name = Panel.name + "_Window";

            closeButton.onClick.AddListener(Panel.Close);
            panelName.text = Panel.PanelName;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
