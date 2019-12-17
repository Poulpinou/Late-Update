using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LateUpdate {
    public class InfosUI : MonoBehaviour
    {
        [Header("Relations")]
        [SerializeField] RectTransform infosPanel;
        [SerializeField] Text nameText;
        [SerializeField] RawImage cameraPreview;

        Actor currentActor; 

        void OnCurrentControllerChanged(Controller controller)
        {
            if(currentActor != null)
            {
                currentActor.FaceCamera.enabled = false;
                cameraPreview.enabled = false;
            }

            Actor actor = null;
            
            if(controller != null)
                actor = controller.GetComponent<Actor>();

            if(actor == null)
            {
                infosPanel.gameObject.SetActive(false);
                currentActor = null;
                return;
            }

            currentActor = actor;
            infosPanel.gameObject.SetActive(true);
            actor.FaceCamera.enabled = true;
            cameraPreview.enabled = true;

            nameText.text = currentActor.Infos.name;
        }

        private void Start()
        {
            OnCurrentControllerChanged(InputManager.CurrentController);
            InputManager.Active.onCurrentControllerChanged.AddListener(OnCurrentControllerChanged);
        }
    }
}
