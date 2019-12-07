using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TinyRPG
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMotor : MonoBehaviour
    {
        #region Private Fields
        Transform target;
        NavMeshAgent agent;
        #endregion

        #region Public Methods
        public void MoveToPoint(Vector3 point)
        {
            agent.SetDestination(point);
        }

        public void FollowTarget(Interactable newTarget)
        {
            agent.stoppingDistance = newTarget.InteractionRadius * 0.8f;
            agent.updateRotation = false;
            target = newTarget.InteractionTransform;
        }

        public void StopFollowingTarget()
        {
            agent.stoppingDistance = 0;
            agent.updateRotation = true;
            target = null;
        }
        #endregion

        #region Private Methods
        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        void OnTargetDestroyed()
        {
            StopFollowingTarget();
        }
        #endregion

        #region Runtime Methods
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
                FaceTarget();
            }
        }
        #endregion       
    }
}
