using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LateUpdate.Stats.UI {
    public class StatPanel : UIPanel
    {
        public StatUIBlock[] categoryBlocks;

        public StatContainer Container { get; private set; }

        public void LinkContainer(StatContainer container)
        {
            if(Container != null)
                Container.onUpdate.RemoveListener(OnContainerUpdate);

            if (container == null)
            {
                Close();
                return;
            }

            Container = container;
            Container.onUpdate.AddListener(OnContainerUpdate);

            for (int i = 0; i < categoryBlocks.Length; i++)
            {
                categoryBlocks[i].Stats = Container.All.Where(s => s.Category == categoryBlocks[i].Category).ToArray();
            }

            Actor actor = container.GetComponent<Actor>();
            if (actor != null)
                PanelName = actor.Infos.name + "'s Stats";
            else
                PanelName = "Stats";
        }

        public override void Close()
        {
            if (Container != null)
                Container.onUpdate.RemoveListener(OnContainerUpdate);

            base.Close();
        }

        protected virtual void OnContainerUpdate()
        {
            RefreshDisplay();
        }

        public virtual void RefreshDisplay()
        {
            for (int i = 0; i < categoryBlocks.Length; i++)
            {
                categoryBlocks[i].RefreshDisplay();
            }
        }

        protected override void OnControlChanged(Controller controller)
        {
            LinkContainer(controller.GetComponent<StatContainer>());
        }
    }
}
