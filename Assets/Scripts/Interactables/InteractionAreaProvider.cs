using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    public class InteractionAreaProvider : MonoBehaviour
    {
        [SerializeField] InteractionArea interactionArea;

        public InteractionArea InteractionArea => interactionArea;

        public void FetchRadiusToCollider(Collider collider = null)
        {
            if(collider == null)
                collider = GetComponent<Collider>();

            if (collider != null)
            {
                interactionArea.ComputeRadiusFromCollider(collider);
            }
            else
                interactionArea.radius = 1;
        }

        private void OnEnable()
        {
            if (interactionArea.point == null)
                interactionArea.point = transform;

            if (interactionArea.radius <= 0)
            {
                FetchRadiusToCollider();
            }
        }

        private void OnValidate()
        {
            if(interactionArea.point == null)
                interactionArea.point = transform;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionArea.point.position, interactionArea.radius);
        }
    }

    [Serializable]
    public struct InteractionArea
    {
        public Transform point;
        public float radius;

        public void ComputeRadiusFromCollider(Collider collider, int offset = 0)
        {
            if (collider == null) return;

            Vector3 extents = collider.bounds.extents;
            radius = Mathf.Max(extents.x, extents.y, extents.z) + offset;
        }

        public bool PointIsInArea(Vector3 point)
        {
            return Vector3.Distance(point, this.point.transform.position) <= radius;
        }
    }
}
