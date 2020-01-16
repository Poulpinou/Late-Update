using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LateUpdate.Actions;
using LateUpdate.Stats;
using LateUpdate.Stats.UI;

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
                Character.Motor.MoveToPoint(hit.point);
                //Character.PerformAction(new MoveToPoint_Action(Character, hit.point));
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

        protected virtual void Update()
        {
            if (IsControlled)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    UIManager.CreateFloatingPanel<StatPanel>(
                        UIManager.DefaultFloatingWindowPosition,
                        p => {
                            p.LinkContainer(GetComponent<StatContainer>());
                        }
                    );
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    Character.Stats.Athletic.AddModifier(new StatModifier(3, StatModifier.ModType.Flat, this));
                }
            }
        }
        #endregion
    }
}
