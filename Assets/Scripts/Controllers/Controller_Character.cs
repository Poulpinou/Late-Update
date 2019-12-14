using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate
{
    [RequireComponent(typeof(Character))]
    public class Controller_Character : Controller
    {
        #region Public Properties
        public Character Character { get; protected set; }
        #endregion

        #region Private Methods
        protected override void AddListeners()
        {
            InputManager.Active.onRightClick.AddListener(OnRightClick);
            InputManager.Active.onRightClickHold.AddListener(OnRightClickHold);
        }

        protected override void RemoveListeners()
        {
            InputManager.Active.onRightClick.RemoveListener(OnRightClick);
            InputManager.Active.onRightClickHold.RemoveListener(OnRightClickHold);
        }

        void OnRightClick(RaycastHit hit)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                ActionManager.OpenActionPopup(Character, interactable);
            }
            else
            {
                Character.Motor.GoTo(hit.point);
            }
        }

        void OnRightClickHold(RaycastHit hit)
        {
            Character.Motor.MoveToPoint(hit.point);
        }
        #endregion

        #region Runtime Methods
        protected virtual void Awake()
        {
            Character = GetComponent<Character>();
        }
        #endregion
    }
}
