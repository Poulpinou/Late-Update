using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate
{
    [RequireComponent(typeof(Motor))]
    public class Controller_Character : Controller
    {
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
                ActionManager.OpenActionPopup(this, interactable);
            }
            else
            {
                Motor.GoTo(hit.point);
            }
        }

        void OnRightClickHold(RaycastHit hit)
        {
            Motor.MoveToPoint(hit.point);
        }
        #endregion
    }
}
