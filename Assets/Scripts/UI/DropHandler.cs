using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace LateUpdate {
    public class DropHandler : MonoBehaviour, IDropHandler
    {
        [Serializable] public class DropEvent : UnityEvent<PointerEventData>{}

        public DropEvent onDrop = new DropEvent();

        public void OnDrop(PointerEventData eventData)
        {
            onDrop.Invoke(eventData);
        }
    }
}
