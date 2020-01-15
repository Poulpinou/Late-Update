using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate.Stats {
    public class Trainer
    {
        TrainableStat stat;
        float frequency;
        float progress;
        int expAmount;

        public Trainer(TrainableStat stat, float frequency, int expAmount = 1)
        {
            this.stat = stat;
            this.frequency = frequency;
            this.expAmount = expAmount;
        }

        public void Update()
        {
            progress += Time.deltaTime;

            if (progress >= frequency)
            {
                progress -= frequency;
                stat.AddExp(1);
            }
        }
    }
}
