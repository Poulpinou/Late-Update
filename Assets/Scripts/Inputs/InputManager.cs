using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace LateUpdate {
    public class InputManager : StaticManager<InputManager>
    {
        #region Serialized Fields
        [Header("Raycasts")]
        [SerializeField] int raycastDepth = 100;
        [SerializeField] LayerMask raycastFilter;

        [Header("Controllers")]
        [SerializeField] Controller defaultController = null;

        [Header("Click")]
        [SerializeField] float holdSensibility = 0.5f;
        #endregion

        #region Private Fields
        Controller currentController = null;
        float holdTime = 0;
        #endregion

        #region Events
        [Serializable] public class CharacterEvent : UnityEvent<Controller> { }
        [Serializable] public class RaycastEvent : UnityEvent<RaycastHit> { }

        [Header("Events")]
        public CharacterEvent onCurrentControllerChanged = new CharacterEvent();
        public RaycastEvent onRightClick = new RaycastEvent();
        public RaycastEvent onRightClickHold = new RaycastEvent();
        public RaycastEvent onRightClickRelease = new RaycastEvent();
        public RaycastEvent onLeftClick = new RaycastEvent();
        public RaycastEvent onLeftClickHold = new RaycastEvent();
        public RaycastEvent onLeftClickRelease = new RaycastEvent();
        #endregion

        #region Static Properties
        public static Controller CurrentController
        {
            get => Active.currentController;
            set => Active.SetCurrentController(value);
        }
        #endregion

        #region Public Methods
        public void SetCurrentController(Controller characterController)
        {
            if (currentController != null)
                currentController.OnControlStop();

            currentController = characterController;

            if (currentController != null)
                currentController.OnControlStart();

            onCurrentControllerChanged.Invoke(currentController);
        }
        #endregion

        #region Private Methods
        void CheckInputsActivity()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0)) LeftClick();
            else if (Input.GetMouseButton(0)) LeftClickHold();
            else if (Input.GetMouseButtonUp(0)) LeftClickRelease();
            else if (Input.GetMouseButtonDown(1)) RightClick();
            else if (Input.GetMouseButton(1)) RightClickHold();
            else if (Input.GetMouseButtonUp(1)) RightClickRelease();
            else if (Input.anyKey) KeyBoard();
        }

        void LeftClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDepth, raycastFilter))
            {
                Controller controller = hit.collider.GetComponent<Controller>();
                if (controller != null)
                {
                    if(controller.ControlledByPlayer && controller != currentController)
                        SetCurrentController(controller);
                }
                else
                {
                    SetCurrentController(null);
                }

                onLeftClick.Invoke(hit);
            }
        }

        void RightClick()
        {
            if (currentController == null) return;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDepth, raycastFilter))
            {
                onRightClick.Invoke(hit);
            }
        }

        void LeftClickHold() {
            if (!CheckHold()) return;
        }

        void RightClickHold() {
            if (!CheckHold()) return;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDepth, raycastFilter))
            {
                onRightClickHold.Invoke(hit);
            }
        }

        void LeftClickRelease()
        {
            holdTime = 0;
        }

        void RightClickRelease()
        {
            holdTime = 0;
        }

        void KeyBoard()
        {

        }

        bool CheckHold()
        {
            if (holdTime > holdSensibility) return true;
            holdTime += Time.deltaTime;
            return false;
        }
        #endregion

        #region Runtime Methods
        private void Update()
        {
            CheckInputsActivity();
        }

        protected override void Awake()
        {
            base.Awake();
            if (defaultController != null)
                SetCurrentController(defaultController);
        }
        #endregion
    }
}
