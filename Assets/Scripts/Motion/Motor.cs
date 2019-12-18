﻿using System.Collections;
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
        [SerializeField] float destinationThreshold = 0.001f;
        [SerializeField] float followDistance = 1f;
        #endregion

        #region Private Fields
        Transform target;
        NavMeshAgent agent;
        Coroutine coroutine;
        #endregion

        #region Public Methods
        /// <summary>
        /// Makes this object move to <paramref name="point"/>
        /// </summary>
        /// <param name="point">The destination</param>
        public void MoveToPoint(Vector3 point)
        {
            agent.SetDestination(point);
        }

        /// <summary>
        /// Makes this object follow <paramref name="target"/>
        /// </summary>
        /// <param name="target">The <see cref="IInteractable"/> target to follow</param>
        public void FollowTarget(IInteractable target)
        {
            coroutine = StartCoroutine(KeepFollowingTarget(target));
        }

        /// <summary>
        /// Makes this object go to <paramref name="point"/> and calls <paramref name="callback"/> when destination is reached
        /// </summary>
        /// <param name="point">The destination</param>
        /// <param name="callback">The action to perform when <paramref name="point"/> is reached</param>
        public void GoTo(Vector3 point, Action callback = null)
        {
            coroutine = StartCoroutine(ReachPoint(point, callback));
        }

        /// <summary>
        /// Makes this object go to <paramref name="target"/> and calls <paramref name="callback"/> when destination is reached
        /// </summary>
        /// <param name="target">The target to reach</param>
        /// <param name="callback">The action to perform when <paramref name="target"/> is reached</param>
        public void GoTo(IInteractable target, Action callback = null)
        {
            coroutine = StartCoroutine(ReachTarget(target, callback));
        }

        /// <summary>
        /// Cancel the current movement and clear <see cref="NavMeshAgent"/>
        /// </summary>
        public void Clear()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            agent.stoppingDistance = 0;
            target = null;
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
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        protected bool PathComplete()
        {
            if (!agent.pathPending && agent.remainingDistance <= destinationThreshold)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    return true;
            }

            return false;
        }
        #endregion

        #region Coroutines
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
        #endregion

        #region Runtime Methods
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        #endregion       
    }
}
