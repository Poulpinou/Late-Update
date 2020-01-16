using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate.Cameras {
    public class RTSCameraController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Relations")]
        [SerializeField] new Camera camera;
        [SerializeField] Transform pivot;

        [Header("Position | Rotation")]
        [SerializeField] Vector3 offset;
        [SerializeField] float pitch = 2f;
        [SerializeField] float yawSpeed = 100f;
        [SerializeField] float moveSpeed = 1;

        [Header("Zoom")]
        [SerializeField] float zoomSpeed = 4f;
        [SerializeField] float minZoom = 5f;
        [SerializeField] float maxZoom = 15f;

        [Header("Raycasting")]
        [SerializeField] LayerMask raycastFilter;
        #endregion

        #region Private Fields
        float currentZoom = 10f;
        float currentYaw = 0f;
        Transform target;
        Controller nearestController;
        #endregion

        #region Public Properties
        public Transform Target
        {
            get => target;
            set
            {
                target = value;
                if(target != null)
                    nearestController = target.GetComponent<Controller>();
            }
        }

        public Camera Camera => camera;
        #endregion

        #region Runtime Methods
        private void Update()
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            currentYaw -= Input.GetAxis("Roll") * yawSpeed * Time.deltaTime;

            if(target != null)
            {
                if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
                    Target = null;
            }

            if (Input.GetButtonDown("Focus") && InputManager.CurrentController != null)
            {
                Target = InputManager.CurrentController.transform;
            }
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                pivot.transform.position = Vector3.Lerp(pivot.transform.position, target.position, Time.deltaTime * moveSpeed);
            }
            else
            {
                RaycastHit hit;
                float yPos = pivot.position.y;
                if (Physics.Raycast(new Vector3(pivot.position.x, 100, pivot.position.z), Vector3.down, out hit, 200, raycastFilter))
                {
                    yPos = hit.point.y;
                }

                Vector3 destination = pivot.position 
                    + Camera.transform.forward * Input.GetAxis("Vertical")
                    + Camera.transform.right * Input.GetAxis("Horizontal");

                destination.y = yPos;

                if(nearestController != null && Vector3.Distance(nearestController.transform.position, destination) > 5)
                {
                    Vector3 fromOriginToObject = destination - nearestController.transform.position;
                    fromOriginToObject *= 5 / Vector3.Distance(nearestController.transform.position, destination);
                    pivot.position = nearestController.transform.position + fromOriginToObject;
                }
                else
                {
                    pivot.position = Vector3.Lerp(pivot.transform.position, destination, Time.deltaTime * moveSpeed);
                }
                    
            }
            
            camera.transform.position = pivot.position - offset * currentZoom;
            camera.transform.LookAt(pivot.position + Vector3.up * pitch);
            camera.transform.RotateAround(pivot.position, Vector3.up, currentYaw);
        }

        private void Start()
        {
            Target = InputManager.CurrentController.transform;
        }
        #endregion

        #region Editor Methods
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(pivot.position, new Vector3(1, 0.1f, 1));
        }
        #endregion

    }
}
