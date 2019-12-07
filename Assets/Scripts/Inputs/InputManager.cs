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
        #endregion

        #region Private Fields
        Controller currentController = null;
        #endregion

        #region Events
        [Serializable] public class CharacterEvent : UnityEvent<Controller> { }
        [Serializable] public class RaycastEvent : UnityEvent<RaycastHit> { }

        [Header("Events")]
        public CharacterEvent onCurrentControllerChanged = new CharacterEvent();
        public RaycastEvent onRightClick = new RaycastEvent();
        public RaycastEvent onLeftClick = new RaycastEvent();
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
            else if (Input.GetMouseButtonDown(1)) RightClick();
            else if (Input.anyKey) KeyBoard();
        }

        void LeftClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDepth, raycastFilter))
            {
                Controller controller = hit.collider.GetComponent<Controller>();
                if (controller != null && controller.ControlledByPlayer && controller != currentController)
                {
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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDepth, raycastFilter))
            {
                onRightClick.Invoke(hit);
            }
        }

        void KeyBoard()
        {

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
