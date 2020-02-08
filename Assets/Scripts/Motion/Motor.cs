using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;
using LateUpdate.Actions;

namespace LateUpdate
{
    /// <summary>
    /// This class is required to move an object and interact with <see cref="NavMeshAgent"/>
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Motor : WorldObjectComponent
    {
        #region Serialized Fields
        [SerializeField] NavMeshAgent agent;
        [SerializeField] float destinationThreshold = 0.001f;
        [SerializeField] float followDistance = 1f;
        #endregion

        #region Private Fields
        IInteractable target;
        #endregion

        #region Public Properties
        public IInteractable Target {
            get => target;
            private set
            {
                target = value;
                if (target != null)
                {
                    TargetArea = target.GetInteractionArea();
                    TargetTransform = TargetArea.point;
                }
                else
                {
                    TargetArea = default;
                    TargetTransform = null;
                }
            }
        }
        public InteractionArea TargetArea { get; private set; }
        public Transform TargetTransform { get; private set; }
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
        protected bool PathComplete()
        {
            if (TargetArea.PointIsInArea(transform.position))
                return true;

            if (!Agent.pathPending && Agent.remainingDistance <= destinationThreshold + Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                    return true;
            }

            return false;
        }
        #endregion

        #region Coroutines
        IEnumerator ReachTarget(IInteractable newTarget, Action callback = null)
        {
            Clear();

            Target = newTarget;
            
            Agent.stoppingDistance = TargetArea.radius;
            
            Agent.SetDestination(TargetArea.point.position);

            while (!PathComplete())
            {
                Agent.SetDestination(TargetArea.point.position);

                yield return null;
            }
            StopFollowingTarget();

            if (callback != null)
                callback.Invoke();
        }

        IEnumerator ReachPoint(Vector3 point, Action callback = null)
        {
            Clear();
            Agent.SetDestination(point);

            yield return new WaitUntil(PathComplete);

            if (callback != null)
                callback.Invoke();
        }

        IEnumerator KeepFollowingTarget(IInteractable newTarget)
        {
            Clear();

            Target = newTarget;
            Agent.stoppingDistance = TargetArea.radius + followDistance;

            while (TargetTransform != null)
            {
                Debug.Log(TargetTransform.position);
                Agent.SetDestination(TargetTransform.position);
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
