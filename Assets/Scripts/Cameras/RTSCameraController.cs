using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        Character nearestCharacter;
        #endregion

        #region Public Properties
        public Transform Target
        {
            get => target;
            set
            {
                target = value;
                if(target != null)
                    nearestCharacter = target.GetComponent<Character>();
            }
        }

        public Camera Camera => camera;
        #endregion

        #region Private Methods
        void FindNearestCharacter()
        {
            nearestCharacter = WorldObjectManager.RequestObject<Character>("Controllable", request: f => f.OrderByDescending(c => c.Stats.LineOfSight.Value - Vector3.Distance(pivot.position, c.transform.position)));
        }

        void ListenInputs()
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            currentYaw -= Input.GetAxis("Roll") * yawSpeed * Time.deltaTime;

            if (target != null)
            {
                if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
                    Target = null;
            }

            if (Input.GetButtonDown("Focus") && InputManager.CurrentController != null)
            {
                Target = InputManager.CurrentController.transform;
            }
        }

        void ComputeTransforms()
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



                if (nearestCharacter == null)
                {
                    FindNearestCharacter();
                    return;
                }

                if (Vector3.Distance(nearestCharacter.transform.position, destination) > nearestCharacter.Stats.LineOfSight.Value)
                {
                    FindNearestCharacter();
                    Vector3 fromOriginToObject = destination - nearestCharacter.transform.position;
                    fromOriginToObject *= (nearestCharacter.Stats.LineOfSight.Value * 0.99f) / Vector3.Distance(nearestCharacter.transform.position, destination);
                    pivot.position = Vector3.Lerp(pivot.position, nearestCharacter.transform.position + fromOriginToObject, Time.deltaTime * moveSpeed);
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
        #endregion

        #region Runtime Methods
        private void Update()
        {
            ListenInputs();
        }

        private void LateUpdate()
        {
            ComputeTransforms();
        }
        #endregion

        #region Editor Methods
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(pivot.position, new Vector3(1, 0.1f, 1));

            if(nearestCharacter != null)
            {
                Gizmos.DrawWireSphere(nearestCharacter.transform.position, nearestCharacter.Stats.LineOfSight.Value);
            }
        }
        #endregion

    }
}
