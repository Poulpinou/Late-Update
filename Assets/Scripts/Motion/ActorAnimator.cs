using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LateUpdate {
    public class ActorAnimator : WorldObjectComponent
    {
        [SerializeField][Range(0,1)] float locomotionAnimationSmoothTime = .1f;

        Animator animator;
        NavMeshAgent agent;

        public AnimatorOverrideController OverrideController { get; private set; }
        public Animator Animator => animator;
        public NavMeshAgent Agent => agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            OverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = OverrideController;
        }

        private void Update()
        {
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
        }
    }
}
