using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace LateUpdate {
    public class CameraManager : StaticManager<CameraManager>
    {
        #region Serialized Fields
        [Header("Camera Manager Settings")]
        [SerializeField] Camera defaultCamera;
        [SerializeField] bool canSwitch = true;
        [SerializeField] bool canAddCamera = true;
        [SerializeField] List<Camera> cameras;
        #endregion

        #region Private Fields
        Camera activeCamera;
        #endregion

        #region Events
        [Serializable] public class CameraEvent : UnityEvent<Camera> { }

        [Header("Events")]
        public CameraEvent onCameraChanged = new CameraEvent();
        #endregion

        #region Static Properties
        public static Camera ActiveCamera
        {
            get => Active.activeCamera;
            set => ChangeActiveCamera(value);
        }

        public static CameraController ActiveController => ActiveCamera.GetComponent<CameraController>();

        public static bool CanSwitchCamera
        {
            get => Active.canSwitch;
            set => Active.canSwitch = value;
        }

        public static bool CanAddCamera => Active.canAddCamera;

        public static List<Camera> CamerasList => Active.cameras;
        #endregion

        #region Static Methods
        public static Camera ChangeActiveCamera(Camera camera)
        {
            if (!CanSwitchCamera)
                return ActiveCamera;

            if (!CamerasList.Contains(camera))
                AddCamera(camera);

            Active.activeCamera = camera;

            foreach (Camera cam in CamerasList)
            {
                if (cam == ActiveCamera)
                    cam.enabled = true;
                else
                    cam.enabled = false;
            }

            Active.onCameraChanged.Invoke(ActiveCamera);

            return ActiveCamera;
        }

        public static void AddCamera(Camera camera)
        {
            if (!CanAddCamera)
                throw new Exception(string.Format("Tried to add {0} to CameraManager but it's not allowed, enable \"Can Add Camera\" to allow this", camera.name));

            Active.cameras.Add(camera);
        }

        public static void RemoveCamera(Camera camera)
        {
            if (CamerasList.Contains(camera))
                Active.cameras.Remove(camera);
        }
        #endregion

        #region Runtime Methods
        protected override void Awake()
        {
            base.Awake();

            if (defaultCamera == null)
                ChangeActiveCamera(Camera.main);
            else
                ChangeActiveCamera(defaultCamera);
        }
        #endregion
    }
}
