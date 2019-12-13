using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LateUpdate {
    public class ActionPopup : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Transform popup;
        [SerializeField] Transform actionButtonsParent;
        [SerializeField] Button actionButtonModel;

        public void Configure(List<GameAction> actions, Vector2 position)
        {
            popup.position = position;

            foreach(GameAction action in actions)
            {
                Button btn = Instantiate(actionButtonModel, actionButtonsParent);
                btn.GetComponentInChildren<Text>().text = action.Name;
                btn.onClick.AddListener(delegate { OnActionClick(action); });
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Destroy(gameObject);
        }

        void OnActionClick(GameAction action)
        {
            action.Run();
            Destroy(gameObject);
        }
    }
}
