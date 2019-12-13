using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace LateUpdate
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Motor : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] float destinationThreshold = 0.001f;
        [SerializeField] float followDistance = 1f;
        #endregion

        #region Private Fields
        Transform target;
        NavMeshAgent agent;
        Coroutine coroutine;
        #endregion

        #region Public Methods
        public void MoveToPoint(Vector3 point)
        {
            agent.SetDestination(point);
        }

        public void FollowTarget(IInteractable newTarget)
        {
            coroutine = StartCoroutine(KeepFollowingTarget(newTarget));
        }

        public void GoTo(Vector3 point, Action callback = null)
        {
            coroutine = StartCoroutine(ReachPoint(point, callback));
        }

        public void GoTo(IInteractable target, Action callback = null)
        {
            coroutine = StartCoroutine(ReachTarget(target, callback));
        }

        IEnumerator ReachTarget(IInteractable newTarget, Action callback = null)
        {
            Clear();

            agent.stoppingDistance = newTarget.InteractionRadius * 0.8f;
            target = newTarget.InteractionTransform;
            agent.SetDestination(target.position);

            while (!PathComplete())
            {
                agent.SetDestination(target.position);

                yield return null;
            }

            StopFollowingTarget();

            if (callback != null)
                callback.Invoke();
        }

        IEnumerator ReachPoint(Vector3 point, Action callback = null)
        {
            Clear();
            agent.SetDestination(point);

            yield return new WaitUntil(PathComplete);

            if (callback != null)
                callback.Invoke();
        }

        IEnumerator KeepFollowingTarget(IInteractable newTarget)
        {
            Clear();
            agent.stoppingDistance = newTarget.InteractionRadius * 0.8f + followDistance;
            target = newTarget.InteractionTransform;

            while (target != null)
            {
                agent.SetDestination(target.position);

                yield return null;
            }
        }

        public void Clear()
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            agent.stoppingDistance = 0;
            target = null;
        }

        public void StopFollowingTarget()
        {
            Clear();
        }

        protected bool PathComplete()
        {
            if (!agent.pathPending && agent.remainingDistance <= destinationThreshold)
            {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    return true;
            }

            return false;
        }
        #endregion

        #region Private Methods
        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        #endregion

        #region Runtime Methods
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        #endregion       
    }
}
