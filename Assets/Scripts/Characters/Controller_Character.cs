using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate
{
    [RequireComponent(typeof(CharacterMotor))]
    public class Controller_Character : Controller
    {
        #region Private Fields
        CharacterMotor motor;
        Interactable focus;
        #endregion

        #region Public Methods
        public void SetFocus(Interactable newFocus)
        {
            if (newFocus != focus)
            {
                if (focus != null)
                    focus.OnDefocused();
                focus = newFocus;
                motor.FollowTarget(newFocus);
            }

            newFocus.OnFocused(transform);
        }

        public void RemoveFocus()
        {
            if (focus != null)
                focus.OnDefocused();
            focus = null;
            motor.StopFollowingTarget();
        }

        public void GoTo(Vector3 position)
        {
            motor.MoveToPoint(position);
        }
        #endregion

        #region Private Methods
        protected override void AddListeners()
        {
            InputManager.Active.onRightClick.AddListener(OnRightClick);
        }

        protected override void RemoveListeners()
        {
            InputManager.Active.onRightClick.RemoveListener(OnRightClick);
        }

        void OnRightClick(RaycastHit hit)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                SetFocus(interactable);
            }
            else
            {
                GoTo(hit.point);
            }
        }
        #endregion

        #region Runtime Methods
        private void Start()
        {
            motor = GetComponent<CharacterMotor>();
        }
        #endregion       
    }
}
