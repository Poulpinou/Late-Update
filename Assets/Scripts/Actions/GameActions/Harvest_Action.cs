using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LateUpdate.Stats;

namespace LateUpdate.Actions {
    public class Harvest_Action : GameAction
    {
        Harvestable harvestable;
        AnimationClip animationClip;
        Inventory inventory;
        Harvest_Stat stat;
        bool isDone = false;
        float harvestTime;

        public override string Name => "Harvest";
        public override float WaitingTime => harvestTime;

        public Harvest_Action(Actor actor, Harvestable harvestable, AnimationClip animationClip) : base(actor, harvestable)
        {
            this.harvestable = harvestable;
            this.animationClip = animationClip;
        }

        protected override void OnStart()
        {
            inventory = Actor.GetComponent<Inventory>();
            if (inventory == null)
            {
                MessageManager.Send(string.Format("{0} has no Inventory attached to it", Actor.Infos.name), LogType.Error);
                Stop();
            }

            stat = Actor.GetComponent<StatContainer>().GetStat<Harvest_Stat>();
            harvestTime = harvestable.ComputeHarvestSpeed(stat.IntValue);
        }

        protected override void InitTrainers()
        {
            base.InitTrainers();
            
            trainers.Add(new Trainer(stat, 1));
        }

        protected override void OnTrain()
        {
            harvestTime = harvestable.ComputeHarvestSpeed(stat.IntValue);
        }

        protected override void OnRun()
        {
            Debug.Log("Harvest");
            if(!harvestable.Harvest(ref inventory))
            {
                isDone = true;
            }
        }

        protected override bool OnDoneCheck()
        {
            return isDone;
        }
    }
}
