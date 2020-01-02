using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    public class UIPanel : MonoBehaviour
    {
        public virtual RectTransform RectTransform { get; protected set; }
        public virtual FloatingWindow FloatingWindow => GetComponentInParent<FloatingWindow>();
        public virtual Func<bool> CloseCondition { get; set; } = null;
        public virtual bool DestroyIfControlChanged => true;
        public virtual string PanelName { get; protected set; }

        public void Close()
        {
            if (FloatingWindow != null)
                Destroy(FloatingWindow.gameObject);
            else
                Destroy(gameObject);
        }

        void OnControlChanged(Controller controller)
        {
            Close();
        }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();

            if (DestroyIfControlChanged)
                InputManager.Active.onCurrentControllerChanged.AddListener(OnControlChanged);

            PanelName = name;
        }

        private void Update()
        {
            if (CloseCondition != null && CloseCondition.Invoke())
                Close();
        }
    }
}
