using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace LateUpdate
{
    /// <summary>
    /// This class is required to move an object and interact with <see cref="NavMeshAgent"/>
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Motor : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] NavMeshAgent agent;
        [SerializeField] float destinationThreshold = 0.001f;
        [SerializeField] float followDistance = 1f;
        #endregion

        #region Public Properties
        public Transform Target { get; private set; }
        public NavMeshAgent Agent => agent;
        #endregion

        #region Public Methods
        /// <summary>
        /// Makes this object move to <paramref name="point"/>
        /// </summary>
        /// <param name="point">The destination</param>
        public void MoveToPoint(Vector3 point)
        {
            Agent.SetDestination(point);
        }

        /// <summary>
        /// Makes this object follow <paramref name="target"/>
        /// </summary>
        /// <param name="target">The <see cref="IInteractable"/> target to follow</param>
        public void FollowTarget(IInteractable target)
        {
            StartCoroutine(KeepFollowingTarget(target));
        }

        /// <summary>
        /// Makes this object go to <paramref name="point"/> and calls <paramref name="callback"/> when destination is reached
        /// </summary>
        /// <param name="point">The destination</param>
        /// <param name="callback">The action to perform when <paramref name="point"/> is reached</param>
        public void GoTo(Vector3 point, Action callback = null)
        {
            StartCoroutine(ReachPoint(point, callback));
        }

        /// <summary>
        /// Makes this object go to <paramref name="target"/> and calls <paramref name="callback"/> when destination is reached
        /// </summary>
        /// <param name="target">The target to reach</param>
        /// <param name="callback">The action to perform when <paramref name="target"/> is reached</param>
        public void GoTo(IInteractable target, Action callback = null)
        {
            StartCoroutine(ReachTarget(target, callback));
        }

        /// <summary>
        /// Cancel the current movement and clear <see cref="NavMeshAgent"/>
        /// </summary>
        public void Clear()
        {
            StopAllCoroutines();

            Agent.SetDestination(transform.position);
            Agent.stoppingDistance = 0;
            Target = null;
        }

        /// <summary>
        /// Makes the object stop following target
        /// </summary>
        public void StopFollowingTarget()
        {
            Clear();
        }
        #endregion

        #region Private Methods
        void FaceTarget()
        {
            Vector3 direction = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        protected bool PathComplete()
        {
            if (!Agent.pathPending && Agent.remainingDistance <= destinationThreshold + Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                    return true;
            }

            return false;
        }
        #endregion

        #region Coroutines
        public IEnumerator ReachTarget(IInteractable newTarget, Action callback = null)
        {
            Clear();

            InteractionArea area = newTarget.GetInteractionArea();

            Agent.stoppingDistance = area.radius;
            Target = area.point;
            Agent.SetDestination(Target.position);

            while (!PathComplete())
            {
                Agent.SetDestination(Target.position);

                yield return null;
            }

            StopFollowingTarget();

            if (callback != null)
                callback.Invoke();
        }

        public IEnumerator ReachPoint(Vector3 point, Action callback = null)
        {
            Clear();
            Agent.SetDestination(point);

            yield return new WaitUntil(PathComplete);

            if (callback != null)
                callback.Invoke();
        }

        public IEnumerator KeepFollowingTarget(IInteractable newTarget)
        {
            Clear();

            InteractionArea area = newTarget.GetInteractionArea();

            Agent.stoppingDistance = area.radius + followDistance;
            Target = area.point;

            while (Target != null)
            {
                Debug.Log("Follow...");
                Agent.SetDestination(Target.position);
                yield return null;
            }
        }
        #endregion

        #region Editor Methods
        private void OnValidate()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();
        }
        #endregion       
    }
}
