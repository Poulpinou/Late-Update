using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// In game camera behaviour
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Relations")]
        [SerializeField] Transform target;

        [Header("Position | Rotation")]
        [SerializeField] Vector3 offset;
        [SerializeField] float pitch = 2f;
        [SerializeField] float yawSpeed = 100f;

        [Header("Zoom")]
        [SerializeField] float zoomSpeed = 4f;
        [SerializeField] float minZoom = 5f;
        [SerializeField] float maxZoom = 15f;
        #endregion

        #region Private Fields
        float currentZoom = 10f;
        float currentYaw = 0f;
        #endregion

        #region Public Properties
        public Transform Target
        {
            get => target;
            set
            {
                target = value;
                //TODO : Add smooth transition
            }
        }
        #endregion

        #region Private Methods
        void OnCurrentControllerChanged(Controller controller)
        {
            target = controller != null? controller.transform : null;
        }
        #endregion

        #region Runtime Methods
        private void Update()
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp( currentZoom, minZoom, maxZoom);
            currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            transform.position = target.position - offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch);
            transform.RotateAround(target.position, Vector3.up, currentYaw);
        }

        private void Start()
        {
            InputManager.Active.onCurrentControllerChanged.AddListener(OnCurrentControllerChanged);
        }
        #endregion
    }
}
