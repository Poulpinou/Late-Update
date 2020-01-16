using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LateUpdate.Cameras {
    public class RTSCameraController : MonoBehaviour
    {
        public enum Mode { free, limitedToLineOfSight, followTarget }

        #region Serialized Fields
        [Header("Settings")]
        [SerializeField] Mode mode;

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
            }
        }

        public Camera Camera => camera;

        public Mode CurrentMode => mode;
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

            if (mode == Mode.followTarget)
            {
                if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
                {
                    mode = Mode.limitedToLineOfSight;
                }
            }

            if (Input.GetButtonDown("Focus"))
            {
                if(target == null)
                    Target = InputManager.CurrentController.transform;
                mode = Mode.followTarget;
            }
        }

        Vector3 GetDestinationFromInputAxis()
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
            return destination;
        }

        void ComputeTransforms()
        {
            switch (mode)
            {
                case Mode.free:
                    pivot.position = Vector3.Lerp(pivot.transform.position, GetDestinationFromInputAxis(), Time.deltaTime * moveSpeed);
                    break;

                case Mode.limitedToLineOfSight:
                    Vector3 destination = GetDestinationFromInputAxis();

                    if (nearestCharacter == null)
                        FindNearestCharacter();

                    if (Vector3.Distance(nearestCharacter.transform.position, destination) > nearestCharacter.Stats.LineOfSight.Value)
                    {
                        FindNearestCharacter();
                        Vector3 fromOriginToObject = destination - nearestCharacter.transform.position;
                        fromOriginToObject *= (nearestCharacter.Stats.LineOfSight.Value - 0.01f) / Vector3.Distance(nearestCharacter.transform.position, destination);
                        pivot.position = Vector3.Lerp(pivot.position, nearestCharacter.transform.position + fromOriginToObject, Time.deltaTime * moveSpeed);
                    }
                    else
                    {
                        pivot.position = Vector3.Lerp(pivot.transform.position, destination, Time.deltaTime * moveSpeed);
                    }
                    break;

                case Mode.followTarget:
                    if (target != null)
                        pivot.transform.position = Vector3.Lerp(pivot.transform.position, target.position, Time.deltaTime * moveSpeed);
                    break;
            }

            camera.transform.position = pivot.position - offset * currentZoom;
            camera.transform.LookAt(pivot.position + Vector3.up * pitch);
            camera.transform.RotateAround(pivot.position, Vector3.up, currentYaw);
        }

        void OnCurrentControllerChanged(Controller controller)
        {
            target = controller != null ? controller.transform : null;
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

        private void Start()
        {
            Target = InputManager.CurrentController.transform;
            InputManager.Active.onCurrentControllerChanged.AddListener(OnCurrentControllerChanged);
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
