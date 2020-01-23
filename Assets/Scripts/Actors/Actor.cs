using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LateUpdate.Stats;
using LateUpdate.Actions;

namespace LateUpdate {
    public class Actor : WorldObject, ITooltipable
    {
        #region Serialized Fields
        [Header("Identity")]
        [SerializeField] ActorInfos infos;
        [SerializeField] Camera faceCamera;
        #endregion

        #region Private Fields
        Coroutine actionCoroutine;
        #endregion

        #region Public Properties
        public ActorInfos Infos => infos;
        public Camera FaceCamera => faceCamera;
        /// <summary>
        /// Reference to <see cref="Motor"/> if it has one
        /// </summary>
        public Motor Motor { get; protected set; }
        /// <summary>
        /// False if <see cref="Controller"/> has no <see cref="Motor"/>
        /// </summary>
        public bool CanMove => Motor != null;
        /// <summary>
        /// The <see cref="GameAction"/> currently performed by this <see cref="Actor"/>
        /// </summary>
        public GameAction CurrentAction { get; protected set; }
        /// <summary>
        /// Returns the content of the tooltip for this object
        /// </summary>
        public string TooltipText => infos.name;
        /// <summary>
        /// Returns the priority in the tooltip
        /// </summary>
        public int Priority => -1;
        /// <summary>
        /// The <see cref="StatContainer"/> attached to this <see cref="GameObject"/>
        /// </summary>
        public StatContainer Stats { get; protected set; }
        public override string[] CollectionTags => new string[] { "Actor" };
        #endregion

        #region Public Methods
        /// <summary>
        /// Tells to the <see cref="Actor"/> which <see cref="GameAction"/> it should perform (it will cancel the previous one if any)
        /// </summary>
        /// <param name="action">The <see cref="GameAction"/> to perform</param>
        public void PerformAction(GameAction action)
        {
            StopAction();

            if (action.NeedsContact == true && !action.Target.CanInteract(this))
            {
                CurrentAction = new MoveTo_Action(
                    this,
                    action.Target, 
                    action
                );
            }
            else
            {
                CurrentAction = action;
            }

            actionCoroutine = StartCoroutine(CurrentAction.Execute(OnCurrentActionDone));
        }

        /// <summary>
        /// Stops the <see cref="CurrentAction"/>
        /// </summary>
        public void StopAction()
        {
            if (CurrentAction == null) return;

            StopCoroutine(actionCoroutine);
            CurrentAction.Stop();
            CurrentAction = null;
        }
        #endregion

        #region Private Methods
        void OnCurrentActionDone(GameAction.ExitStatus exitStatus)
        {
            //Debug.Log(CurrentAction.Name + " => " +(CurrentAction.HasNextAction? CurrentAction.NextAction.Name : "none"));
            GameAction lastAction = CurrentAction;
            CurrentAction = null;
            StopCoroutine(actionCoroutine);

            if (exitStatus == GameAction.ExitStatus.hasNextAction && lastAction.NextAction != null)
                PerformAction(lastAction.NextAction);
        }
        #endregion

        #region Runtime Methods
        protected override void Awake()
        {
            base.Awake();
            Motor = GetComponent<Motor>();
            Stats = GetComponent<StatContainer>();
            Stats.InitStats();
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.name = GetType().Name + "_" + Infos.name;
        }
#endif
        #endregion

        #region Internal Classes
        [Serializable]
        public struct ActorInfos
        {
            public string name;
            [TextArea]
            public string description;
            [TextArea]
            public string[] routineSentences;
        }
        #endregion
    }
}
